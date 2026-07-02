namespace DeliveryAPI.Models.Response
{
    public class TransferStockLogDetailModel
    {
        public int TransitID { get; set; }
        public DateTime? TransferOutDate { get; set; }
        public DateTime? TransferOutTime { get; set; }
        public string? SourceBranch { get; set; }
        public string? DeliveryNoteNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public string? IMEI { get; set; }
        public string? TransferredOutBy { get; set; }
        public string? TransferStatus { get; set; }
        public int TransferQty { get; set; }
        public string? DestinationBranch { get; set; }
        public DateTime? TransferInTime { get; set; }
        public string? InwardDoneBy { get; set; }
        public string? TransferDuration { get; set; }
    }
}
