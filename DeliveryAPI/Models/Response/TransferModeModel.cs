public class TransferModeModel
{
    public int TransferModeId { get; set; }

    public string TransferModeCode { get; set; } = string.Empty;

    public string TransferModeName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}