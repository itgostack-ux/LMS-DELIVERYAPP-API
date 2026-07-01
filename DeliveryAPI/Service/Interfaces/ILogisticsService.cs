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
    }
}