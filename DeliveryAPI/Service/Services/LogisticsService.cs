using DeliveryAPI.Models.Response;
using DeliveryAPI.Repository.Interfaces;
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
    }
}