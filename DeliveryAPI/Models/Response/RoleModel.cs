namespace DeliveryAPI.Models.Response
{
    public class RoleModel
    {
        public int RoleID { get; set; }

        public string RoleName { get; set; } = string.Empty;
    }


    public class RoleModellifecycle
    {
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int RoleID { get; set; }

        public string? RoleName { get; set; }
    }
}