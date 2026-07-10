using DeliveryAPI.Models.Response;

namespace DeliveryAPI.Repository.Interfaces
{
    public interface ILogisticsRepository
    {
        List<CompanyModel> GetCompanies();

        List<LocationTypeModel> GetLocationTypes(int companyId);

        List<LocationModel> GetLocations(int companyId, int locationTypeId);
        List<RoleModel> GetRoles();

        List<UserModel> GetUsers();

        List<CourierModel> GetCouriers();
        List<DeliveryLifecycleModel> GetDeliveryLifecycles();
        bool SaveDeliveryLifecycle(DeliveryLifecycleModel model);

        // Company User Lifecycle Access
        List<CompanyUserLifecycleAccessViewModel> GetCompanyUserLifecycleAccess();

        bool SaveCompanyUserLifecycleAccess(CompanyUserLifecycleAccessModel model);

        dynamic GetCompanyUserRole(int userId, int companyId);


        List<RoleLifecycleMappingViewModel> GetRoleLifecycleMappings();

        string SaveRoleLifecycleMapping(RoleLifecycleMappingModel model);


        List<TransferStockLogDetailModel> GetTransferStockLogDetailDelivery(
    int companyId,
    string? locationIds,
    DateTime? fromDate,
    DateTime? toDate,
    string? locationTypeIds);

        List<TransferModeModel> GetTransferModes();

        List<DeliveryOrderTimelineModel> GetDeliveryOrderTimeline();
        List<DeliveryOrderTransactionModel> GetDeliveryOrderTransactions();

        bool SaveDeliveryOrderTransaction(DeliveryOrderTransactionModel model);

        List<TransferManifestModelresponse> GetManifestOrders();

        bool SaveTransferManifest(TransferManifestModel model);

        List<DeliveryLifecycleModel> GetRoleBasedLifecycles(int roleId);

        List<RoleModellifecycle> GetUserRoles(int userId);

        string GetNextManifestNo();

    }



}

