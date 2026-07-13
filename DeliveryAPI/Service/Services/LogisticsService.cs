using DeliveryAPI.Models.Response;
using DeliveryAPI.Repository.Interfaces;
using DeliveryAPI.Repository.Repositories;
using DeliveryAPI.Service.Interfaces;

namespace DeliveryAPI.Service.Services
{
    public class LogisticsService : ILogisticsService
    {
        private readonly ILogisticsRepository _repository;

        public LogisticsService(ILogisticsRepository repository)
        {
            _repository = repository;
        }

        public List<CompanyModel> GetCompanies()
        {
            return _repository.GetCompanies();
        }

        public List<LocationTypeModel> GetLocationTypes(int companyId)
        {
            return _repository.GetLocationTypes(companyId);
        }

        public List<LocationModel> GetLocations(int companyId, int locationTypeId)
        {
            return _repository.GetLocations(companyId, locationTypeId);
        }

        public List<RoleModel> GetRoles()
        {
            return _repository.GetRoles();
        }

        public List<UserModel> GetUsers()
        {
            return _repository.GetUsers();
        }

        public List<CourierModel> GetCouriers()
        {
            return _repository.GetCouriers();
        }

        public List<DeliveryLifecycleModel> GetDeliveryLifecycles()
        {
            return _repository.GetDeliveryLifecycles();
        }

        public bool SaveDeliveryLifecycle(DeliveryLifecycleModel model)
        {
            return _repository.SaveDeliveryLifecycle(model);
        }


        public List<CompanyUserLifecycleAccessViewModel> GetCompanyUserLifecycleAccess()
        {
            return _repository.GetCompanyUserLifecycleAccess();
        }

        public bool SaveCompanyUserLifecycleAccess(CompanyUserLifecycleAccessModel model)
        {
            return _repository.SaveCompanyUserLifecycleAccess(model);
        }

        public dynamic GetCompanyUserRole(int companyId, int userId)
        {
            return _repository.GetCompanyUserRole(companyId, userId);
        }

        public List<RoleLifecycleMappingViewModel> GetRoleLifecycleMappings()
        {
            return _repository.GetRoleLifecycleMappings();
        }

        public string SaveRoleLifecycleMapping(RoleLifecycleMappingModel model)
        {
            return _repository.SaveRoleLifecycleMapping(model);
        }

        public List<TransferStockLogDetailModel> GetTransferStockLogDetailDelivery(
    int companyId,
    string? locationIds,
    DateTime? fromDate,
    DateTime? toDate,
    string? locationTypeIds)
        {
            return _repository.GetTransferStockLogDetailDelivery(
                companyId,
                locationIds,
                fromDate,
                toDate,
         
                
                locationTypeIds);
        }



        public List<DeliveryOrderTimelineModel> GetDeliveryOrderTimeline()
        {
            return _repository.GetDeliveryOrderTimeline();
        }

        public List<TransferModeModel> GetTransferModes()
        {
            return _repository.GetTransferModes();
        }


        public List<DeliveryOrderTransactionModel> GetDeliveryOrderTransactions()
        {
            return _repository.GetDeliveryOrderTransactions();
        }

        public bool SaveDeliveryOrderTransaction(DeliveryOrderTransactionModel model)
        {
            return _repository.SaveDeliveryOrderTransaction(model);
        }


public List<TransferManifestModelresponse> GetManifestOrders()
        {
            return _repository.GetManifestOrders();
        }


        public bool SaveTransferManifest(TransferManifestModel model)
        {
            return _repository.SaveTransferManifest(model);
        }

        public List<DeliveryLifecycleModel> GetRoleBasedLifecycles(int roleId)
        {
            return _repository.GetRoleBasedLifecycles(roleId);
        }

        public List<RoleModellifecycle> GetUserRoles(int userId)
        {
            return _repository.GetUserRoles(userId);
        }
        public string GetNextManifestNo()
        {
            return _repository.GetNextManifestNo();
        }

        public List<UserCompanyLocationModel> GetUserCompanyLocations(int userId)
        {
            return _repository.GetUserCompanyLocations(userId);
        }

        public List<UserModel> GetReceiverUsers(int companyId, int locationId)
        {
            return _repository.GetReceiverUsers(companyId, locationId);
        }
    }
}