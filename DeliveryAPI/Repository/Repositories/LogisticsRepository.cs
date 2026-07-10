using Dapper;
using DeliveryAPI.Models.Response;
using DeliveryAPI.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DeliveryAPI.Repository.Repositories
{
    public class LogisticsRepository : ILogisticsRepository
    {
        private readonly string _winCommonConnection;
        private readonly string _winPosConnection;
        private readonly string _biReportsConnection;
        public LogisticsRepository(IConfiguration configuration)
        {
            _winCommonConnection = configuration.GetConnectionString("DefaultConnection")!;
            _winPosConnection = configuration.GetConnectionString("WinPosConnection")!;

            _biReportsConnection = configuration.GetConnectionString("BIReportsConnection")!;
        }

        private IDbConnection CreateWinCommonConnection()
        {
            return new SqlConnection(_winCommonConnection);
        }

        private IDbConnection CreateWinPosConnection()
        {
            return new SqlConnection(_winPosConnection);
        }



        private IDbConnection CreateBIReportsConnection()
        {
            return new SqlConnection(_biReportsConnection);
        }
        #region Company

        public List<CompanyModel> GetCompanies()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
                SELECT
                    CompId,
                    CompName
                FROM CompanyMaster
                WHERE CompActive='Y'
                ORDER BY CompName";

            return db.Query<CompanyModel>(query).ToList();
        }

        #endregion

        #region Location Type

        public List<LocationTypeModel> GetLocationTypes(int companyId)
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
                SELECT DISTINCT
                    LT.LocationTypeId,
                    LT.LocationTypeDesc
                FROM CompanyLocationLink CL
                INNER JOIN LocationTypeMaster LT
                    ON LT.LocationTypeId=CL.LocationTypeId
                INNER JOIN CompanyMaster CM
                    ON CM.CompId=CL.CompId
                WHERE
                    CL.CompId=@CompanyId
                    AND CL.IsActive=1
                    AND LT.IsActive=1
                    AND CM.CompActive='Y'
                ORDER BY LT.LocationTypeDesc";

            return db.Query<LocationTypeModel>(
                query,
                new { CompanyId = companyId }).ToList();
        }

        #endregion

        #region Location

        public List<LocationModel> GetLocations(int companyId, int locationTypeId)
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
                SELECT
                    LM.LocId,
                    LM.LocDesc,
                    LM.StateId,
                    CL.CompId,
                    CL.LocationTypeId
                FROM CompanyLocationLink CL
                INNER JOIN LocationMaster LM
                    ON LM.LocId=CL.LocId
                INNER JOIN CompanyMaster CM
                    ON CM.CompId=CL.CompId
                WHERE
                    CL.CompId=@CompanyId
                    AND CL.LocationTypeId=@LocationTypeId
                    AND CL.IsActive=1
                    AND CM.CompActive='Y'
                ORDER BY LM.LocDesc";

            return db.Query<LocationModel>(
                query,
                new
                {
                    CompanyId = companyId,
                    LocationTypeId = locationTypeId
                }).ToList();
        }

        #endregion

        #region Role

        public List<RoleModel> GetRoles()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
                SELECT
                    RoleID,
                    RoleName
                FROM Roles
                WHERE IsActive=1
                ORDER BY RoleName";

            return db.Query<RoleModel>(query).ToList();
        }

        #endregion

        #region User

        public List<UserModel> GetUsers()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
                SELECT
                    UserId,
                    FullName,
                    LoginName,
                    EmailId,
                    MobileNo
                FROM UserMast
                WHERE
                    Deleted='N'
                    AND StActive='Y'
                ORDER BY FullName";

            return db.Query<UserModel>(query).ToList();
        }

        #endregion

        #region Courier

        public List<CourierModel> GetCouriers()
        {
            using var db = CreateWinPosConnection();

            const string query = @"
                SELECT
                    CourierId,
                    CourierName,
                    TransStateId
                FROM CourierMaster
                ORDER BY CourierName";

            return db.Query<CourierModel>(query).ToList();
        }

        #endregion


        public List<DeliveryLifecycleModel> GetDeliveryLifecycles()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
        SELECT
            LifecycleId,
            SequenceNo,
            StatusCode,
            StatusName,
            NextStatusCode,
            ColorCode,
            Description,
            IsActive,
            CreatedBy,
            CreatedDate,
            ModifiedBy,
            ModifiedDate
        FROM DeliveryLifecycleMaster
        WHERE IsActive = 1
        ORDER BY SequenceNo;";

            return db.Query<DeliveryLifecycleModel>(query).ToList();
        }

        public bool SaveDeliveryLifecycle(DeliveryLifecycleModel model)
        {
            using var db = CreateWinCommonConnection();

            // Duplicate Check (Insert & Update only)
            if (model.IsActive)
            {
                const string duplicateQuery = @"
        SELECT COUNT(1)
        FROM DeliveryLifecycleMaster
        WHERE IsActive = 1
        AND
        (
            UPPER(StatusCode)=UPPER(@StatusCode)
            OR UPPER(StatusName)=UPPER(@StatusName)
        )
        AND LifecycleId <> @LifecycleId";

                int duplicate = db.ExecuteScalar<int>(duplicateQuery, new
                {
                    model.StatusCode,
                    model.StatusName,
                    model.LifecycleId
                });

                if (duplicate > 0)
                    return false;
            }

            string query;

            // INSERT
            if (model.LifecycleId == 0)
            {
                query = @"
        INSERT INTO DeliveryLifecycleMaster
        (
            SequenceNo,
            StatusCode,
            StatusName,
            NextStatusCode,
            ColorCode,
            Description,
            IsActive,
            CreatedBy,
            CreatedDate
        )
        VALUES
        (
            @SequenceNo,
            @StatusCode,
            @StatusName,
            @NextStatusCode,
            @ColorCode,
            @Description,
            1,
            @CreatedBy,
            GETDATE()
        )";
            }
            // DELETE (Soft Delete)
            else if (!model.IsActive)
            {
                query = @"
        UPDATE DeliveryLifecycleMaster
        SET
            IsActive = 0,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE LifecycleId = @LifecycleId";
            }
            // UPDATE
            else
            {
                query = @"
        UPDATE DeliveryLifecycleMaster
        SET
            SequenceNo = @SequenceNo,
            StatusCode = @StatusCode,
            StatusName = @StatusName,
            NextStatusCode = @NextStatusCode,
            ColorCode = @ColorCode,
            Description = @Description,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE LifecycleId = @LifecycleId";
            }

            return db.Execute(query, model) > 0;
        }

        public bool SaveCompanyUserLifecycleAccess(CompanyUserLifecycleAccessModel model)
        {
            using var db = CreateWinCommonConnection();

            // Duplicate Check
            const string duplicateQuery = @"
    SELECT COUNT(1)
    FROM CompanyUserLifecycleAccess
    WHERE CompanyId = @CompanyId
      AND UserId = @UserId
      AND RoleId = @RoleId
      AND IsActive = 1
      AND MappingId <> @MappingId";

            int duplicate = db.ExecuteScalar<int>(duplicateQuery, new
            {
                model.CompanyId,
                model.UserId,
                model.RoleId,
                model.MappingId
            });

            if (duplicate > 0)
                return false;

            string query;

            // INSERT
            if (model.MappingId == 0)
            {
                query = @"
        INSERT INTO CompanyUserLifecycleAccess
        (
            CompanyId,
            UserId,
            RoleId,
            IsActive,
            CreatedBy,
            CreatedDate
        )
        VALUES
        (
            @CompanyId,
            @UserId,
            @RoleId,
            1,
            @CreatedBy,
            GETDATE()
        )";
            }
            // DELETE (Soft Delete)
            else if (!model.IsActive)
            {
                query = @"
        UPDATE CompanyUserLifecycleAccess
        SET
            IsActive = 0,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE MappingId = @MappingId";
            }
            // UPDATE
            else
            {
                query = @"
        UPDATE CompanyUserLifecycleAccess
        SET
            CompanyId = @CompanyId,
            UserId = @UserId,
            RoleId = @RoleId,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE MappingId = @MappingId";
            }

            return db.Execute(query, model) > 0;
        }
        public List<CompanyUserLifecycleAccessViewModel> GetCompanyUserLifecycleAccess()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT

    CULA.MappingId,

    CULA.CompanyId,
    CM.CompName AS CompanyName,

    CULA.UserId,
    UM.FullName AS UserName,

    CULA.RoleId,
    RM.RoleName,

    CULA.IsActive

FROM CompanyUserLifecycleAccess CULA

INNER JOIN CompanyMaster CM
    ON CULA.CompanyId = CM.CompId

INNER JOIN UserMast UM
    ON CULA.UserId = UM.UserId

INNER JOIN Roles RM
    ON CULA.RoleId = RM.RoleID

WHERE
    CULA.IsActive = 1

ORDER BY

    CM.CompName,
    UM.FullName,
    RM.RoleName;";

            return db.Query<CompanyUserLifecycleAccessViewModel>(query).ToList();
        }


        public dynamic GetCompanyUserRole(int userId, int companyId)
        {
            using var db = CreateWinCommonConnection();

            string query;

            object param;

            // Step 1 : User -> Company
            if (companyId == 0)
            {
                query = @"
        SELECT DISTINCT

            CM.CompId,
            CM.CompName

        FROM UserCompLink UCL

        INNER JOIN CompanyMaster CM
            ON UCL.CompId = CM.CompId

        WHERE

            UCL.UserId = @UserId
            AND UCL.AllowAccess = 'Y'
            AND CM.CompActive = 'Y'

        ORDER BY CM.CompName";

                param = new
                {
                    UserId = userId
                };

                return db.Query<CompanyModel>(query, param).ToList();
            }

            // Step 2 : Company + User -> Roles
            query = @"
    SELECT DISTINCT

        R.RoleID,
        R.RoleName

    FROM UserApplicationRolesLink URL

    INNER JOIN Roles R
        ON URL.RoleID = R.RoleID

    INNER JOIN UserCompLink UCL
        ON URL.UserID = UCL.UserId

    WHERE

        URL.UserID = @UserId
        AND UCL.CompId = @CompanyId
        AND URL.IsActive = 1
        AND UCL.AllowAccess = 'Y'
        AND R.IsActive = 1

    ORDER BY R.RoleName";

            param = new
            {
                UserId = userId,
                CompanyId = companyId
            };

            return db.Query<RoleModel>(query, param).ToList();
        }



        public List<RoleLifecycleMappingViewModel> GetRoleLifecycleMappings()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT

    RLM.MappingId,

    RLM.RoleId,

    R.RoleName,

    RLM.LifecycleId,

    DLM.StatusName,

    DLM.SequenceNo,

    RLM.CanView,

    RLM.CanCreate,

    RLM.CanEdit,

    RLM.CanDelete,

    RLM.CanChangeStatus,

    RLM.IsActive

FROM RoleLifecycleMapping RLM

INNER JOIN Roles R
ON RLM.RoleId=R.RoleID

INNER JOIN DeliveryLifecycleMaster DLM
ON RLM.LifecycleId=DLM.LifecycleId

WHERE RLM.IsActive=1

ORDER BY
R.RoleName,
DLM.SequenceNo";

            return db.Query<RoleLifecycleMappingViewModel>(query).ToList();
        }


        public string SaveRoleLifecycleMapping(RoleLifecycleMappingModel model)
        {
            using var db = CreateWinCommonConnection();

            const string duplicate = @"

SELECT COUNT(1)

FROM RoleLifecycleMapping

WHERE

RoleId=@RoleId

AND LifecycleId=@LifecycleId

AND IsActive=1

AND MappingId<>@MappingId";

            int count = db.ExecuteScalar<int>(duplicate, new
            {
                model.RoleId,
                model.LifecycleId,
                model.MappingId
            });

            if (count > 0)
                return "Role already mapped to this Lifecycle.";

            string query;

            if (model.MappingId == 0)
            {
                query = @"

INSERT INTO RoleLifecycleMapping
(
RoleId,
LifecycleId,
CanView,
CanCreate,
CanEdit,
CanDelete,
CanChangeStatus,
IsActive,
CreatedBy,
CreatedDate
)

VALUES
(
@RoleId,
@LifecycleId,
@CanView,
@CanCreate,
@CanEdit,
@CanDelete,
@CanChangeStatus,
1,
@CreatedBy,
GETDATE()
)";
            }
            else if (!model.IsActive)
            {
                query = @"

UPDATE RoleLifecycleMapping

SET

IsActive=0,

ModifiedBy=@ModifiedBy,

ModifiedDate=GETDATE()

WHERE MappingId=@MappingId";
            }
            else
            {
                query = @"

UPDATE RoleLifecycleMapping

SET

RoleId=@RoleId,

LifecycleId=@LifecycleId,

CanView=@CanView,

CanCreate=@CanCreate,

CanEdit=@CanEdit,

CanDelete=@CanDelete,

CanChangeStatus=@CanChangeStatus,

ModifiedBy=@ModifiedBy,

ModifiedDate=GETDATE()

WHERE MappingId=@MappingId";
            }

            db.Execute(query, model);

            if (model.MappingId == 0)
                return "Saved Successfully.";

            if (!model.IsActive)
                return "Deleted Successfully.";

            return "Updated Successfully.";
        }


        public List<DeliveryOrderTimelineModel> GetDeliveryOrderTimeline()
        {
            using var db = CreateWinCommonConnection();

            return db.Query<DeliveryOrderTimelineModel>(
                "USP_GetDeliveryOrderTimeline",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public List<TransferStockLogDetailModel> GetTransferStockLogDetailDelivery(
            int companyId,
            string? locationIds,
            DateTime? fromDate,
            DateTime? toDate,
            string? locationTypeIds)
        {
            using var db = CreateBIReportsConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CompId", companyId);
            parameters.Add("@LocationId", locationIds);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@LocationTypeId", locationTypeIds);

            return db.Query<TransferStockLogDetailModel>(
                "RP_TransferStockLog_Detail_delivery_08_09",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 180   // 3 minutes
            ).ToList();
        }
        public List<TransferModeModel> GetTransferModes()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT

TransferModeId,
TransferModeCode,
TransferModeName,
Description,
IsActive,
CreatedBy,
CreatedDate,
ModifiedBy,
ModifiedDate

FROM TransferModeMaster

WHERE IsActive=1

ORDER BY TransferModeName";

            return db.Query<TransferModeModel>(query).ToList();
        }


        public List<DeliveryOrderTransactionModel> GetDeliveryOrderTransactions()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT *

FROM DeliveryOrderTransaction

WHERE IsActive=1

ORDER BY TransferOrderId DESC";

            return db.Query<DeliveryOrderTransactionModel>(query).ToList();
        }


        public bool SaveDeliveryOrderTransaction(DeliveryOrderTransactionModel model)
        {
            using var db = CreateWinCommonConnection();
            db.Open();

            using var tran = db.BeginTransaction();

            try
            {
                string query;

                // INSERT
                if (model.TransferOrderId == 0)
                {
                    query = @"
INSERT INTO DeliveryOrderTransaction
(
    TransitID,
    DeliveryNoteNo,
    TransferOutDate,
    TransferOutTime,

    SourceLocationId,
    SourceLocationName,

    DestinationLocationId,
    DestinationLocationName,

    ItemCode,
    ItemName,
    IMEI,
    TransferQty,

    LifecycleId,
    LifecycleSequenceNo,
    LifecycleCode,
    LifecycleName,

    TransferModeId,
    TransferModeName,

    TransferOutById,
    TransferOutByName,

AssignedById,
AssignedByName,
AssignedDate,

    CourierId,
    CourierName,

    AWBBillNo,

    TransferInTime,

    InwardDoneById,
    InwardDoneByName,

    TransferDuration,

    Remarks,

    IsActive,

    CreatedBy,
    CreatedByName,
    CreatedDate,

    OtherPartyType,
    VehicleNo,
    OtherPartyName,

    CompanyId,
    CompanyName,

    PickupManifestId,
    PickupManifestNo,

    SourceLocationTypeId,
    SourceLocationTypeName,

    DestinationLocationTypeId,
    DestinationLocationTypeName
)
VALUES
(
    @TransitID,
    @DeliveryNoteNo,
    @TransferOutDate,
    @TransferOutTime,

    @SourceLocationId,
    @SourceLocationName,

    @DestinationLocationId,
    @DestinationLocationName,

    @ItemCode,
    @ItemName,
    @IMEI,
    @TransferQty,

    @LifecycleId,
    @LifecycleSequenceNo,
    @LifecycleCode,
    @LifecycleName,

    @TransferModeId,
    @TransferModeName,

    @TransferOutById,
    @TransferOutByName,
@AssignedById,
@AssignedByName,
GETDATE(),

    @CourierId,
    @CourierName,

    @AWBBillNo,

    @TransferInTime,

    @InwardDoneById,
    @InwardDoneByName,

    @TransferDuration,

    @Remarks,

    1,

    @CreatedBy,
    @CreatedByName,
    GETDATE(),

    @OtherPartyType,
    @VehicleNo,
    @OtherPartyName,

    @CompanyId,
    @CompanyName,

    @PickupManifestId,
    @PickupManifestNo,

    @SourceLocationTypeId,
    @SourceLocationTypeName,

    @DestinationLocationTypeId,
    @DestinationLocationTypeName
);

SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                    model.TransferOrderId = db.ExecuteScalar<long>(query, model, tran);
                }
                // SOFT DELETE
                else if (!model.IsActive)
                {
                    query = @"
UPDATE DeliveryOrderTransaction
SET
    IsActive = 0,
    ModifiedBy = @ModifiedBy,
    ModifiedByName = @ModifiedByName,
    ModifiedDate = GETDATE()
WHERE TransferOrderId = @TransferOrderId";

                    db.Execute(query, model, tran);
                }
                // UPDATE
                else
                {
                    query = @"
UPDATE DeliveryOrderTransaction
SET
    LifecycleId = @LifecycleId,
    LifecycleSequenceNo = @LifecycleSequenceNo,
    LifecycleCode = @LifecycleCode,
    LifecycleName = @LifecycleName,

    TransferModeId = @TransferModeId,
    TransferModeName = @TransferModeName,

    TransferOutById = @TransferOutById,
    TransferOutByName = @TransferOutByName,
AssignedUserId = @AssignedUserId,
AssignedUserName = @AssignedUserName,

AssignedById = @AssignedById,
AssignedByName = @AssignedByName,
AssignedDate = GETDATE(),

    CourierId = @CourierId,
    CourierName = @CourierName,

    AWBBillNo = @AWBBillNo,

    TransferInTime = @TransferInTime,

    InwardDoneById = @InwardDoneById,
    InwardDoneByName = @InwardDoneByName,

    TransferDuration = @TransferDuration,

    Remarks = @Remarks,

    OtherPartyType = @OtherPartyType,
    VehicleNo = @VehicleNo,
    OtherPartyName = @OtherPartyName,

    CompanyId = @CompanyId,
    CompanyName = @CompanyName,

    PickupManifestId = @PickupManifestId,
    PickupManifestNo = @PickupManifestNo,

    SourceLocationTypeId = @SourceLocationTypeId,
    SourceLocationTypeName = @SourceLocationTypeName,

    DestinationLocationTypeId = @DestinationLocationTypeId,
    DestinationLocationTypeName = @DestinationLocationTypeName,

    ModifiedBy = @ModifiedBy,
    ModifiedByName = @ModifiedByName,
    ModifiedDate = GETDATE()

WHERE TransferOrderId = @TransferOrderId";

                    db.Execute(query, model, tran);
                }

                //=========================================================
                // Close Previous Lifecycle Log
                //=========================================================
                db.Execute(@"
UPDATE DeliveryOrderLifecycleLog
SET
    StatusEndTime = GETDATE(),
    DurationMinutes = DATEDIFF(MINUTE, StatusStartTime, GETDATE()),

    ModifiedBy = @ModifiedBy,
    ModifiedByName = @ModifiedByName,
    ModifiedDate = GETDATE()

WHERE TransferOrderId = @TransferOrderId
AND StatusEndTime IS NULL",
                model, tran);
                //=========================================================
                // Insert New Lifecycle Log
                //=========================================================

                db.Execute(@"
INSERT INTO DeliveryOrderLifecycleLog
(
    TransferOrderId,
    ManifestId,
    ManifestNo,

    LifecycleId,
    LifecycleSequenceNo,
    LifecycleCode,
    LifecycleName,

    StatusStartTime,
    StatusEndTime,
    DurationMinutes,

    ChangedById,
    ChangedByName,

    Remarks,

    CreatedBy,
    CreatedByName,
    CreatedDate,

    ModifiedBy,
    ModifiedByName,
    ModifiedDate
)
VALUES
(
    @TransferOrderId,
    @PickupManifestId,
    @PickupManifestNo,

    @LifecycleId,
    @LifecycleSequenceNo,
    @LifecycleCode,
    @LifecycleName,

    GETDATE(),
    NULL,
    NULL,

    @ModifiedBy,
    @ModifiedByName,

    @Remarks,

    @CreatedBy,
    @CreatedByName,
    GETDATE(),

    @ModifiedBy,
    @ModifiedByName,
    GETDATE()
)", model, tran);

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }










        public List<TransferManifestModelresponse> GetManifestOrders()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
SELECT

    -- TransferManifest
    TM.ManifestId,
    TM.ManifestNo,
    TM.TransferOrderId,

    TM.AssignedUserId,
    TM.AssignedUserName,

    TM.ReceiverUserId,
    TM.ReceiverUserName,

    TM.OTP,

    TM.LifecycleId,
    TM.LifecycleSequenceNo,
    TM.LifecycleCode,
    TM.LifecycleName,

    TM.ManifestDate,
    TM.Status,

    -- DeliveryOrderTransaction
    DOT.TransitID,
    DOT.DeliveryNoteNo,
    DOT.TransferOutDate,
    DOT.TransferOutTime,

    DOT.SourceLocationId,
    DOT.SourceLocationName,

    DOT.DestinationLocationId,
    DOT.DestinationLocationName,

    DOT.ItemCode,
    DOT.ItemName,
    DOT.IMEI,
    DOT.TransferQty,

    -- Delivery Lifecycle (Aliased)
    DOT.LifecycleId AS DOTLifecycleId,
    DOT.LifecycleSequenceNo AS DOTLifecycleSequenceNo,
    DOT.LifecycleCode AS DOTLifecycleCode,
    DOT.LifecycleName AS DOTLifecycleName,

    DOT.TransferModeId,
    DOT.TransferModeName,

    -- Delivery Assigned User (Aliased)
    DOT.AssignedUserId AS DOTAssignedUserId,
    DOT.AssignedUserName AS DOTAssignedUserName,

    DOT.CourierId,
    DOT.CourierName,
    DOT.AWBBillNo,

    DOT.TransferInTime,

    DOT.InwardDoneById,
    DOT.InwardDoneByName,

    DOT.TransferDuration,
    DOT.Remarks,

    DOT.IsActive,
    DOT.CreatedBy,
    DOT.CreatedByName,
    DOT.CreatedDate,
    DOT.ModifiedBy,
    DOT.ModifiedByName,
    DOT.ModifiedDate,

    DOT.TransferOutById,
    DOT.TransferOutByName,

    DOT.OtherPartyType,
    DOT.VehicleNo,
    DOT.OtherPartyName,

    DOT.CompanyId,
    DOT.CompanyName,

    DOT.PickupManifestId,
    DOT.PickupManifestNo,

    DOT.SourceLocationTypeId,
    DOT.SourceLocationTypeName,

    DOT.DestinationLocationTypeId,
    DOT.DestinationLocationTypeName,

    DOT.LocationTypeId,
    DOT.LocationTypeName

FROM TransferManifest TM
INNER JOIN DeliveryOrderTransaction DOT
    ON TM.TransferOrderId = DOT.TransferOrderId

ORDER BY
    TM.ManifestId DESC,
    TM.TransferOrderId;";

            return db.Query<TransferManifestModelresponse>(query).ToList();
        }
        public bool SaveTransferManifest(TransferManifestModel model)
        {
            using var db = CreateWinCommonConnection();

            db.Open();

            using var tran = db.BeginTransaction();

            try
            {
                //=========================================================
                // NEW MANIFEST
                //=========================================================
                if (model.ManifestId == 0)
                {
                    model.ManifestId = db.ExecuteScalar<int>(@"

INSERT INTO TransferManifest
(
    ManifestNo,
    TransferOrderId,

    AssignedUserId,
    AssignedUserName,

    ReceiverUserId,
    ReceiverUserName,

    OTP,

    LifecycleId,
    LifecycleSequenceNo,
    LifecycleCode,
    LifecycleName,

    ManifestDate,
    Status,

    CreatedBy,
    CreatedByName,
    CreatedDate,

    ModifiedBy,
    ModifiedByName,
    ModifiedDate,

    AssignedById,
    AssignedByName,
    AssignedDate
)
VALUES
(
    @ManifestNo,
    @TransferOrderId,

    @AssignedUserId,
    @AssignedUserName,

    @ReceiverUserId,
    @ReceiverUserName,

    @OTP,

    @LifecycleId,
    @LifecycleSequenceNo,
    @LifecycleCode,
    @LifecycleName,

    GETDATE(),
    @Status,

    @CreatedBy,
    @CreatedByName,
    GETDATE(),

    @CreatedBy,
    @CreatedByName,
    GETDATE(),

    @CreatedBy,
    @CreatedByName,
    GETDATE()
);

SELECT CAST(SCOPE_IDENTITY() AS INT);

", model, tran);
                }
                else
                {
                    //=========================================================
                    // UPDATE MANIFEST
                    //=========================================================

                    db.Execute(@"

UPDATE TransferManifest
SET

AssignedUserId=@AssignedUserId,
AssignedUserName=@AssignedUserName,

ReceiverUserId=@ReceiverUserId,
ReceiverUserName=@ReceiverUserName,

OTP=@OTP,

LifecycleId=@LifecycleId,
LifecycleSequenceNo=@LifecycleSequenceNo,
LifecycleCode=@LifecycleCode,
LifecycleName=@LifecycleName,

ManifestDate=@ManifestDate,
Status=@Status,

AssignedById=@ModifiedBy,
AssignedByName=@ModifiedByName,
AssignedDate=GETDATE(),

ModifiedBy=@ModifiedBy,
ModifiedByName=@ModifiedByName,
ModifiedDate=GETDATE()

WHERE ManifestId=@ManifestId

", model, tran);
                }

                //---------------------------------------------------------
                // CLOSE PREVIOUS LOG
                //---------------------------------------------------------

                db.Execute(@"

UPDATE TransferManifestLifecycleLog
SET

StatusEndTime=GETDATE(),

DurationMinutes=
DATEDIFF
(
MINUTE,
StatusStartTime,
GETDATE()
),

ModifiedBy=@ModifiedBy,
ModifiedByName=@ModifiedByName,
ModifiedDate=GETDATE()

WHERE ManifestId=@ManifestId
AND StatusEndTime IS NULL

", model, tran);

                //---------------------------------------------------------
                // INSERT NEW STATUS LOG
                //---------------------------------------------------------

                db.Execute(@"

INSERT INTO TransferManifestLifecycleLog
(
ManifestId,
ManifestNo,
TransferOrderId,

LifecycleId,
LifecycleSequenceNo,
LifecycleCode,
LifecycleName,

StatusStartTime,
StatusEndTime,
DurationMinutes,

ChangedById,
ChangedByName,

Remarks,

CreatedBy,
CreatedByName,
CreatedDate,

AssignedById,
AssignedByName,
AssignedDate,

ModifiedBy,
ModifiedByName,
ModifiedDate
)
VALUES
(
@ManifestId,
@ManifestNo,
@TransferOrderId,

@LifecycleId,
@LifecycleSequenceNo,
@LifecycleCode,
@LifecycleName,

GETDATE(),
NULL,
NULL,

@ModifiedBy,
@ModifiedByName,

@Status,

@CreatedBy,
@CreatedByName,
GETDATE(),

@ModifiedBy,
@ModifiedByName,
GETDATE(),

@ModifiedBy,
@ModifiedByName,
GETDATE()
)

", model, tran);

                tran.Commit();

                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }








































        public List<DeliveryLifecycleModel> GetRoleBasedLifecycles(int roleId)
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT
    DLM.LifecycleId,
    DLM.SequenceNo,
    DLM.StatusCode,
    DLM.StatusName,
    DLM.NextStatusCode,
    DLM.ColorCode ,
    DLM.Description,
    DLM.IsActive,
    DLM.CreatedBy,
    DLM.CreatedDate,
    DLM.ModifiedBy,
    DLM.ModifiedDate

FROM RoleLifecycleMapping RLM

INNER JOIN DeliveryLifecycleMaster DLM
    ON RLM.LifecycleId = DLM.LifecycleId

WHERE
    RLM.RoleId = @RoleId
    AND RLM.IsActive = 1
    AND DLM.IsActive = 1
    AND RLM.CanView = 1

ORDER BY DLM.SequenceNo";

            return db.Query<DeliveryLifecycleModel>(
                query,
                new { RoleId = roleId }
            ).ToList();
        }
        public List<RoleModellifecycle> GetUserRoles(int userId)
        {
            using var db = CreateWinCommonConnection();

            const string query = @"

SELECT DISTINCT
    CU.UserId,
    CU.CompanyId,
    R.RoleID,
    R.RoleName

FROM CompanyUserLifecycleAccess CU

INNER JOIN Roles R
    ON CU.RoleId = R.RoleID

WHERE
    CU.UserId = @UserId
    AND CU.IsActive = 1
    AND R.IsActive = 1

ORDER BY R.RoleName";

            return db.Query<RoleModellifecycle>(
                query,
                new { UserId = userId }
            ).ToList();
        }

        public string GetNextManifestNo()
        {
            using var db = CreateWinCommonConnection();

            const string query = @"
SELECT
    'MAN-' +
    FORMAT(GETDATE(),'ddMMyy') +
    '-' +
    RIGHT(
        '000' +
        CAST(
            ISNULL(
                MAX(
                    CAST(RIGHT(ManifestNo,3) AS INT)
                ),
            0
            ) + 1
        AS VARCHAR(3)),
    3)
FROM TransferManifest
WHERE ManifestNo IS NOT NULL
AND LTRIM(RTRIM(ManifestNo)) <> ''
AND ManifestNo LIKE 'MAN-' + FORMAT(GETDATE(),'ddMMyy') + '-%';
";

            return db.QueryFirstOrDefault<string>(query)
                   ?? $"MAN-{DateTime.Now:ddMMyy}-001";
        }

    }




    }