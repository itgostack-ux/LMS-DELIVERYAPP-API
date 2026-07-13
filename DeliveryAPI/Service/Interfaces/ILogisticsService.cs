using DeliveryAPI.Models.Response;

namespace DeliveryAPI.Service.Interfaces
{
    public interface ILogisticsService
    {
        List<CompanyModel> GetCompanies();

        List<LocationTypeModel> GetLocationTypes(int companyId);

        List<LocationModel> GetLocations(int companyId, int locationTypeId);


        List<RoleModel> GetRoles();

        List<UserModel> GetUsers();

        List<CourierModel> GetCouriers();

        List<DeliveryLifecycleModel> GetDeliveryLifecycles();

        bool SaveDeliveryLifecycle(DeliveryLifecycleModel model);

        List<CompanyUserLifecycleAccessViewModel> GetCompanyUserLifecycleAccess();

        bool SaveCompanyUserLifecycleAccess(CompanyUserLifecycleAccessModel model);

        dynamic GetCompanyUserRole(int userId, int companyId);

        List<RoleLifecycleMappingViewModel> GetRoleLifecycleMappings();

        List<DeliveryOrderTimelineModel> GetDeliveryOrderTimeline();

        string SaveRoleLifecycleMapping(RoleLifecycleMappingModel model);

        List<TransferStockLogDetailModel> GetTransferStockLogDetailDelivery(
     int companyId,
     string? locationIds,
     DateTime? fromDate,
     DateTime? toDate,
     string? locationTypeIds);

        List<TransferModeModel> GetTransferModes();

        List<DeliveryOrderTransactionModel> GetDeliveryOrderTransactions();

        bool SaveDeliveryOrderTransaction(DeliveryOrderTransactionModel model);
        List<TransferManifestModelresponse> GetManifestOrders();
        bool SaveTransferManifest(TransferManifestModel model);

        List<DeliveryLifecycleModel> GetRoleBasedLifecycles(int roleId);

        List<RoleModellifecycle> GetUserRoles(int userId);

        string GetNextManifestNo();
        List<UserCompanyLocationModel> GetUserCompanyLocations(int userId);

        List<UserModel> GetReceiverUsers(int companyId, int locationId);
    }


}