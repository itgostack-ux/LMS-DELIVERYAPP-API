namespace DeliveryAPI.Models.Response
{
    public class TransferStockLogDetailModel
    {
        // Company
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        // Transfer
        public int TransitID { get; set; }
        public DateTime? TransferOutDate { get; set; }
        public DateTime? TransferOutTime { get; set; }

        public int SourceLocationId { get; set; }
        public string? SourceBranch { get; set; }
        public int? SourceLocationTypeId { get; set; }
        public string? SourceLocationTypeName { get; set; }

        public string? DeliveryNoteNo { get; set; }

        // Item
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public string? IMEI { get; set; }




        // Transfer Out
        public int TransferOutByUserId { get; set; }
        public string? TransferredOutBy { get; set; }

        public string? TransferStatus { get; set; }
        public int? TransferQty { get; set; }

        public int AcceptedQty { get; set; }           // 1 if received, else 0
        public int PendingQty { get; set; }            // 1 if pending, else 0

        // Destination
        public int DestinationLocationId { get; set; }
        public string? DestinationBranch { get; set; }
        public int? DestinationLocationTypeId { get; set; }
        public string? DestinationLocationTypeName { get; set; }

        // Transfer In
        public DateTime? TransferInTime { get; set; }
        public int? InwardDoneByUserId { get; set; }
        public string? InwardDoneBy { get; set; }

        public string? TransferDuration { get; set; }

        // Logistics
        public int TransferOrderId { get; set; }

        public int LifecycleId { get; set; }
        public int LifecycleSequenceNo { get; set; }
        public string? LifecycleCode { get; set; }
        public string? LifecycleName { get; set; }

        public string? LogisticsStatus { get; set; }

        // Transfer Mode
        public int TransferModeId { get; set; }
        public string? TransferModeName { get; set; }

        // Driver
        public int? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }

        // Courier
        public int? CourierId { get; set; }
        public string? CourierName { get; set; }
        public string? AWBBillNo { get; set; }

        // Remarks
        public string? Remarks { get; set; }

        // Audit
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}