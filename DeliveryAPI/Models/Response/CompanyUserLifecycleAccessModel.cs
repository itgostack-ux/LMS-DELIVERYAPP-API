namespace DeliveryAPI.Models.Response
{
    public class CompanyUserLifecycleAccessModel
    {
        public int MappingId { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
    public class CompanyUserLifecycleAccessViewModel
    {
        public int MappingId { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}