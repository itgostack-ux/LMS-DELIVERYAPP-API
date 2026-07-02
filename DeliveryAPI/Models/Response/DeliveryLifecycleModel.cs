namespace DeliveryAPI.Models.Response
{
    public class DeliveryLifecycleModel
    {
        public int LifecycleId { get; set; }

        public int SequenceNo { get; set; }

        public string StatusCode { get; set; } = string.Empty;

        public string StatusName { get; set; } = string.Empty;

        public string? NextStatusCode { get; set; }

        public string? ColorCode { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}