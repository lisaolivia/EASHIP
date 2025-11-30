namespace Eaship.Models;

public enum ContractStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}

public class Contract
{
    public long ContractId { get; set; }
    public long BookingId { get; set; }

    public long? TongkangId { get; set; }
    public long? TugboatId { get; set; }





    public string? PdfUrl { get; set; }
    public ContractStatus Status { get; private set; } = ContractStatus.Pending;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }


    // NAVIGATION
    public Booking? Booking { get; set; }
    public Tongkang? Tongkang { get; set; }
    public Tugboat? Tugboat { get; set; }


    // --- METHODS (simple) ---
    public void MarkPending()
    {
        Status = ContractStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        Status = ContractStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = ContractStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = ContractStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
