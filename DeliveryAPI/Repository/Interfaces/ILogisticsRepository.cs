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

    }
}

