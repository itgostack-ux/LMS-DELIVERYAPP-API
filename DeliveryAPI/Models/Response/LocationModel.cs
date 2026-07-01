namespace DeliveryAPI.Models.Response
{
    public class LocationModel
    {
        public int LocId { get; set; }

        public string LocDesc { get; set; } = string.Empty;

        public int StateId { get; set; }

        public int CompId { get; set; }

        public int LocationTypeId { get; set; }
    }
}