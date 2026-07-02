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
                "RP_TransferStockLog_Detail_delivery",
                parameters,
                commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }


    }