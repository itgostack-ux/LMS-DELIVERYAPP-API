namespace DeliveryAPI.Models.Response
{
    public class RoleLifecycleMappingModel
    {
        public int MappingId { get; set; }

        public int RoleId { get; set; }

        public int LifecycleId { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanChangeStatus { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    public class RoleLifecycleMappingViewModel
    {
        public int MappingId { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public int LifecycleId { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public int SequenceNo { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanChangeStatus { get; set; }

        public bool IsActive { get; set; }
    }
}