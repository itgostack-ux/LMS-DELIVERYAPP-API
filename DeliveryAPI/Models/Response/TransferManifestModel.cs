namespace DeliveryAPI.Models.Response
{
    public class TransferManifestModel 
    { 
        public int ManifestId  { get; set; }
        public string? ManifestNo { get; set; } 
        public int TransferOrderId { get; set; }
        public int AssignedUserId { get; set; } 
        public string? AssignedUserName { get; set; }
        public int ReceiverUserId { get; set; }
        public string? ReceiverUserName { get; set; }
        public string? OTP { get; set; } 
        public int LifecycleId { get; set; } 
        public int LifecycleSequenceNo { get; set; }
        public string? LifecycleCode { get; set; } 
        public string? LifecycleName { get; set; }
        public DateTime? ManifestDate { get; set; } 
        public string? Status { get; set; } }
}


namespace DeliveryAPI.Models.Response
{
    public class TransferManifestModelresponse
    {
        // TransferManifest
        public int ManifestId { get; set; }
        public string? ManifestNo { get; set; }
        public int TransferOrderId { get; set; }

        public int AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }

        public int ReceiverUserId { get; set; }
        public string? ReceiverUserName { get; set; }

        public string? OTP { get; set; }

        public int LifecycleId { get; set; }
        public int LifecycleSequenceNo { get; set; }
        public string? LifecycleCode { get; set; }
        public string? LifecycleName { get; set; }

        public DateTime? ManifestDate { get; set; }
        public string? Status { get; set; }

        // DeliveryOrderTransaction
        public string? TransitID { get; set; }
        public string? DeliveryNoteNo { get; set; }

        public DateTime? TransferOutDate { get; set; }
        public DateTime? TransferOutTime { get; set; }

        public int SourceLocationId { get; set; }
        public string? SourceLocationName { get; set; }

        public int DestinationLocationId { get; set; }
        public string? DestinationLocationName { get; set; }

        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? IMEI { get; set; }

        public decimal TransferQty { get; set; }

        public int TransferModeId { get; set; }
        public string? TransferModeName { get; set; }

        public int? CourierId { get; set; }
        public string? CourierName { get; set; }

        public string? AWBBillNo { get; set; }

        public DateTime? TransferInTime { get; set; }

        public int? InwardDoneById { get; set; }
        public string? InwardDoneByName { get; set; }

        public string? TransferDuration { get; set; }
        public string? Remarks { get; set; }

        public string? VehicleNo { get; set; }
        public string? OtherPartyName { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int LocationTypeId { get; set; }
        public string? LocationTypeName { get; set; }

        public int? PickupManifestId { get; set; }
        public string? PickupManifestNo { get; set; }
    }
}
