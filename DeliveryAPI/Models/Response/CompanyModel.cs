namespace DeliveryAPI.Models.Response
{
    public class CompanyModel
    {
        public int CompId { get; set; }

        public string CompName { get; set; } = string.Empty;

        public string? CompShortCode { get; set; }

        public string? TradeName { get; set; }
    }
}