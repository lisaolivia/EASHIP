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
    public int ContractId { get; set; }
    public int BookingId { get; set; }
    public string PdfUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ContractStatus Status { get; set; } = ContractStatus.Pending;
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? ApprovedAt { get; set; }



    //method

    public void Approve(string approver)
    {
        if (Status != ContractStatus.Pending)
            throw new InvalidOperationException("Hanya kontrak Pending yang bisa di-approve.");

        Status = ContractStatus.Approved;
        ApprovedBy = approver;
        ApprovedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Renter menolak kontrak.
    /// </summary>
    public void Reject(string approver)
    {
        if (Status != ContractStatus.Pending)
            throw new InvalidOperationException("Hanya kontrak Pending yang bisa ditolak.");

        Status = ContractStatus.Rejected;
        ApprovedBy = approver;
        ApprovedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Admin/system membatalkan kontrak (hanya jika belum Approved).
    /// </summary>
    public void CancelContract()
    {
        if (Status == ContractStatus.Approved)
            throw new InvalidOperationException("Kontrak yang sudah Approved tidak bisa dibatalkan.");

        Status = ContractStatus.Cancelled;
    }
    /// <summary>
    /// Admin mengirim kontrak ke renter → status jadi Pending.
    /// </summary>
    public void SendForApproval()
    {
        if (Status != ContractStatus.Pending)
            throw new InvalidOperationException("Hanya kontrak Pending yang bisa dikirim.");
        if (string.IsNullOrEmpty(PdfUrl))
            throw new InvalidOperationException("PDF harus dibuat sebelum kontrak dikirim.");

        Status = ContractStatus.Pending;
    }

    public void GeneratePdf(string pdfUrl)
    {
        if (string.IsNullOrWhiteSpace(pdfUrl))
            throw new ArgumentException("PDF URL wajib diisi.");
        PdfUrl = pdfUrl;
    }




}
