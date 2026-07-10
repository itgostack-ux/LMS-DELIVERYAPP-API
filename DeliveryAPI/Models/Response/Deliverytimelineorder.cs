public class DeliveryOrderTimelineModel
{
    //==================================================
    // Delivery Order
    //==================================================
    public long TransferOrderId { get; set; }
    public long? TransitID { get; set; }
    public string? DeliveryNoteNo { get; set; }

    public DateTime? TransferOutDate { get; set; }
    public DateTime? TransferOutTime { get; set; }

    public string? ItemCode { get; set; }
    public string? ItemName { get; set; }
    public string? IMEI { get; set; }
    public int? TransferQty { get; set; }

    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }

    public int? SourceLocationId { get; set; }
    public string? SourceLocationName { get; set; }
    public int? SourceLocationTypeId { get; set; }
    public string? SourceLocationTypeName { get; set; }

    public int? DestinationLocationId { get; set; }
    public string? DestinationLocationName { get; set; }
    public int? DestinationLocationTypeId { get; set; }
    public string? DestinationLocationTypeName { get; set; }

    public int? TransferModeId { get; set; }
    public string? TransferModeName { get; set; }

    public int? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }

    public int? AssignedById { get; set; }
    public string? AssignedByName { get; set; }
    public DateTime? AssignedDate { get; set; }

    public int? TransferOutById { get; set; }
    public string? TransferOutByName { get; set; }

    public int? CourierId { get; set; }
    public string? CourierName { get; set; }

    public string? AWBBillNo { get; set; }

    public DateTime? TransferInTime { get; set; }

    public int? InwardDoneById { get; set; }
    public string? InwardDoneByName { get; set; }

    public string? TransferDuration { get; set; }

    public string? Remarks { get; set; }

    public int? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }
    public string? ModifiedByName { get; set; }
    public DateTime? ModifiedDate { get; set; }

    //==================================================
    // Manifest
    //==================================================
    public int? ManifestId { get; set; }
    public string? ManifestNo { get; set; }

    public int? ManifestAssignedUserId { get; set; }
    public string? ManifestAssignedUserName { get; set; }

    public int? ManifestAssignedById { get; set; }
    public string? ManifestAssignedByName { get; set; }
    public DateTime? ManifestAssignedDate { get; set; }

    public int? ReceiverUserId { get; set; }
    public string? ReceiverUserName { get; set; }

    public string? OTP { get; set; }

    public DateTime? ManifestDate { get; set; }

    public int? ManifestCreatedBy { get; set; }
    public string? ManifestCreatedByName { get; set; }
    public DateTime? ManifestCreatedDate { get; set; }

    public int? ManifestModifiedBy { get; set; }
    public string? ManifestModifiedByName { get; set; }
    public DateTime? ManifestModifiedDate { get; set; }

    //==================================================
    // Lifecycle Master
    //==================================================
    public int LifecycleId { get; set; }
    public int SequenceNo { get; set; }
    public string? StatusCode { get; set; }
    public string? StatusName { get; set; }
    public string? ColorCode { get; set; }
    public string? Description { get; set; }

    //==================================================
    // Delivery Order Lifecycle Log
    //==================================================
    public int? OrderLogId { get; set; }

    public DateTime? OrderStatusStartTime { get; set; }
    public DateTime? OrderStatusEndTime { get; set; }
    public int? OrderDurationMinutes { get; set; }

    public int? OrderChangedById { get; set; }
    public string? OrderChangedByName { get; set; }

    public int? OrderCreatedBy { get; set; }
    public string? OrderCreatedByName { get; set; }
    public DateTime? OrderCreatedDate { get; set; }

    public int? OrderModifiedBy { get; set; }
    public string? OrderModifiedByName { get; set; }
    public DateTime? OrderModifiedDate { get; set; }

    //==================================================
    // Manifest Lifecycle Log
    //==================================================
    public int? ManifestLogId { get; set; }

    public DateTime? ManifestStatusStartTime { get; set; }
    public DateTime? ManifestStatusEndTime { get; set; }
    public int? ManifestDurationMinutes { get; set; }

    public int? ManifestChangedById { get; set; }
    public string? ManifestChangedByName { get; set; }

    public int? ManifestCreatedByLog { get; set; }
    public string? ManifestCreatedByNameLog { get; set; }
    public DateTime? ManifestCreatedDateLog { get; set; }

    public int? ManifestAssignedByIdLog { get; set; }
    public string? ManifestAssignedByNameLog { get; set; }
    public DateTime? ManifestAssignedDateLog { get; set; }

    public int? ManifestModifiedByLog { get; set; }
    public string? ManifestModifiedByNameLog { get; set; }
    public DateTime? ManifestModifiedDateLog { get; set; }

    //==================================================
    // Timeline Status
    //==================================================
    public string? OrderStatus { get; set; }
    public string? ManifestStatus { get; set; }
}