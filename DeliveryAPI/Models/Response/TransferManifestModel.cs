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
        public string? Status { get; set; }

        public int CreatedBy { get; set; }
        public string?CreatedByName { get; set; }

        public int ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }

        public int AssignedById { get; set; }
        public string? AssignedByName { get; set; }

    }
}





namespace DeliveryAPI.Models.Response
{
    public class TransferManifestModelresponse
    {
        // ==========================
        // TransferManifest
        // ==========================
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

        // ==========================
        // DeliveryOrderTransaction
        // ==========================
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

        // DeliveryOrderTransaction Lifecycle (Aliased)
        public int? DOTLifecycleId { get; set; }
        public int? DOTLifecycleSequenceNo { get; set; }
        public string? DOTLifecycleCode { get; set; }
        public string? DOTLifecycleName { get; set; }

        public int TransferModeId { get; set; }
        public string? TransferModeName { get; set; }

        // DeliveryOrderTransaction Assigned User (Aliased)
        public int? DOTAssignedUserId { get; set; }
        public string? DOTAssignedUserName { get; set; }

        public int? CourierId { get; set; }
        public string? CourierName { get; set; }

        public string? AWBBillNo { get; set; }

        public DateTime? TransferInTime { get; set; }

        public int? InwardDoneById { get; set; }
        public string? InwardDoneByName { get; set; }

        public string? TransferDuration { get; set; }
        public string? Remarks { get; set; }

        // Audit Fields
        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Transfer Out
        public int? TransferOutById { get; set; }
        public string? TransferOutByName { get; set; }

        // Other Party
        public string? OtherPartyType { get; set; }

        public string? VehicleNo { get; set; }
        public string? OtherPartyName { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        // Pickup Manifest
        public int? PickupManifestId { get; set; }
        public string? PickupManifestNo { get; set; }

        // Source Location Type
        public int? SourceLocationTypeId { get; set; }
        public string? SourceLocationTypeName { get; set; }

        // Destination Location Type
        public int? DestinationLocationTypeId { get; set; }
        public string? DestinationLocationTypeName { get; set; }

        // Location Type
        public int LocationTypeId { get; set; }
        public string? LocationTypeName { get; set; }
    }
}
