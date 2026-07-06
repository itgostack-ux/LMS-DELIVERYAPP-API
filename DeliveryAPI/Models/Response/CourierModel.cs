namespace DeliveryAPI.Models.Response
{
    public class CourierModel
    {
        public int CourierId { get; set; }

        public string CourierName { get; set; } = string.Empty;

        public int TransStateId { get; set; }
    }
}