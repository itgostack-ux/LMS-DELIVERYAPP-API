namespace DeliveryAPI.Models.Response
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string LoginName { get; set; } = string.Empty;

        public string? EmailId { get; set; }

        public string? MobileNo { get; set; }
    }
}