using Dapper;
using DeliveryAPI.Models.Response;
using DeliveryAPI.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DeliveryAPI.Repository.Repositories
{
    public class LogisticsRepository : ILogisticsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public LogisticsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public List<CompanyModel> GetCompanies()
        {
            const string query = @"
                SELECT
                    CompId,
                    CompName
                FROM CompanyMaster
                WHERE CompActive = 'Y'
                ORDER BY CompName;";

            using var db = CreateConnection();
            return db.Query<CompanyModel>(query).ToList();
        }

        public List<LocationTypeModel> GetLocationTypes(int companyId)
        {
            using var db = CreateConnection();

            const string query = @"
        SELECT DISTINCT
            LT.LocationTypeId,
            LT.LocationTypeDesc
        FROM CompanyLocationLink CL
        INNER JOIN LocationTypeMaster LT
            ON CL.LocationTypeId = LT.LocationTypeId
        INNER JOIN CompanyMaster CM
            ON CL.CompId = CM.CompId
        WHERE
            CL.CompId = @CompanyId
            AND CL.IsActive = 1
            AND LT.IsActive = 1
            AND CM.CompActive = 'Y'
        ORDER BY LT.LocationTypeDesc;";

            return db.Query<LocationTypeModel>(
                query,
                new { CompanyId = companyId }).ToList();
        }

        public List<LocationModel> GetLocations(int companyId, int locationTypeId)
        {
            using var db = CreateConnection();

            const string query = @"
        SELECT
            LM.LocId,
            LM.LocDesc,
            LM.StateId,
            CL.CompId,
            CL.LocationTypeId
        FROM CompanyLocationLink CL
        INNER JOIN LocationMaster LM
            ON CL.LocId = LM.LocId
        INNER JOIN CompanyMaster CM
            ON CL.CompId = CM.CompId
        WHERE
            CL.CompId = @CompanyId
            AND CL.LocationTypeId = @LocationTypeId
            AND CL.IsActive = 1
            AND CM.CompActive = 'Y'
        ORDER BY LM.LocDesc;";

            return db.Query<LocationModel>(
                query,
                new
                {
                    CompanyId = companyId,
                    LocationTypeId = locationTypeId
                }).ToList();
        }


        public List<RoleModel> GetRoles()
        {
            using var db = CreateConnection();

            const string query = @"
        SELECT
            RoleID,
            RoleName
        FROM Roles
        WHERE IsActive = 1
        ORDER BY RoleName;";

            return db.Query<RoleModel>(query).ToList();
        }

        public List<UserModel> GetUsers()
        {
            using var db = CreateConnection();

            const string query = @"
        SELECT
            UserId,
            FullName,
            LoginName,
            EmailId,
            MobileNo
        FROM UserMast
        WHERE
            Deleted = 'N'
            AND StActive = 'Y'
        ORDER BY FullName;";

            return db.Query<UserModel>(query).ToList();
        }
    }
}