namespace DeliveryAPI.Models.Response
{
    public class UserCompanyLocationModel
    {
        public int UserId { get; set; }
        public int CompId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int LocId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string AllowAccess { get; set; } = string.Empty;
    }
}
