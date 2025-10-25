namespace Eaship.Models;

public enum ContractStatus
{
    Pending,   // menunggu tanda tangan
    Approved,  // kedua pihak sudah tanda tangan
    Rejected,  // ditolak oleh salah satu pihak
    Cancelled  // dibatalkan (sebelum Approved)
}

public class Contract
{

    private Contract() { }

    public long ContractId { get; set; }
    public long BookingId { get; private set; }

    public string PdfUrl { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public ContractStatus Status { get; private set; } = ContractStatus.Pending;


    // Tanda tangan renter
    public bool RenterSigned { get; private set; }
    public string? RenterSigner { get; private set; }
    public DateTime? RenterSignedAt { get; private set; }

    // Tanda tangan owner/company (diwakili admin)
    public bool OwnerSigned { get; private set; }
    public string? OwnerSigner { get; private set; }
    public DateTime? OwnerSignedAt { get; private set; }

    // Waktu final approved (setelah kedua pihak sign)
    public DateTime? ApprovedAt { get; private set; }

    public static Contract CreateFrom(Booking booking, string pdfUrl)
    {
        if (booking is null) throw new ArgumentNullException(nameof(booking));
        if (booking.Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Contract hanya dari Booking yang CONFIRMED.");
        if (string.IsNullOrWhiteSpace(pdfUrl))
            throw new ArgumentException("PDF URL wajib diisi.", nameof(pdfUrl));

        return new Contract
        {
            BookingId = booking.BookingId,
            PdfUrl = pdfUrl.Trim(),
            Status = ContractStatus.Pending
        };
    }

    public void GeneratePdf(string pdfUrl)
    {
        if (Status != ContractStatus.Pending)
            throw new InvalidOperationException("PDF hanya bisa dibuat saat Pending.");
        if (RenterSigned || OwnerSigned)
            throw new InvalidOperationException("Tidak bisa ganti PDF setelah ada tanda tangan.");
        if (string.IsNullOrWhiteSpace(pdfUrl))
            throw new ArgumentException("PDF URL wajib diisi.", nameof(pdfUrl));

        PdfUrl = pdfUrl.Trim();
        Touch();
    }

    public void SignByRenter(string renterName /*, User renter untuk role-check */)
    {
        EnsurePending();
        EnsurePdf();
        if (RenterSigned) return;

        RenterSigned = true;
        RenterSigner = renterName;
        RenterSignedAt = DateTime.UtcNow;
        Touch();
        MaybeApprove();
    }

    public void SignByOwner(string adminName /*, User admin untuk role-check */)
    {
        EnsurePending();
        EnsurePdf();
        if (OwnerSigned) return;

        OwnerSigned = true;
        OwnerSigner = adminName;
        OwnerSignedAt = DateTime.UtcNow;
        Touch();
        MaybeApprove();
    }

    public void Reject(string by /*, User user untuk role-check */, string reason = "")
    {
        EnsurePending();
        Status = ContractStatus.Rejected;
        // (opsional) simpan reason & siapa yang reject ke field terpisah
        Touch();
    }

    public void Cancel(string by /*, User admin untuk role-check */)
    {
        if (Status == ContractStatus.Approved)
            throw new InvalidOperationException("Kontrak yang sudah Approved tidak bisa dibatalkan.");
        Status = ContractStatus.Cancelled;
        Touch();
    }

    // --- helpers ---
    private void MaybeApprove()
    {
        if (RenterSigned && OwnerSigned)
        {
            Status = ContractStatus.Approved;
            ApprovedAt = DateTime.UtcNow;
            Touch();
        }
    }



    private void EnsurePending()
    {
        if (Status != ContractStatus.Pending)
            throw new InvalidOperationException("Aksi ini hanya untuk kontrak Pending.");
    }

    private void EnsurePdf()
    {
        if (string.IsNullOrWhiteSpace(PdfUrl))
            throw new InvalidOperationException("PDF harus dibuat sebelum tanda tangan.");
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
