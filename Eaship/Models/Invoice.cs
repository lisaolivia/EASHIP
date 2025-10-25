namespace Eaship.Models;

public enum InvoiceStatus { Draft, Sent, Paid, Cancelled }

public class Invoice
{
    private Invoice() { }

    public int InvoiceId { get; private set; }
    public long ContractId { get; private set; }

    public string Number { get; private set; } = string.Empty;   // di-set via SetNumber()
    public decimal Amount { get; private set; }
    public DateTime IssuedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public string PdfUrl { get; private set; } = string.Empty;

    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;

    // Factory: tanpa nomor (biar nomor diisi setelah SaveChanges -> pakai InvoiceId)
    public static Invoice CreateFromContract(Contract contract, decimal amount, DateTime issuedAtUtc, DateTime? dueDateUtc, string pdfUrl)
    {
        if (contract is null) throw new ArgumentNullException(nameof(contract));
        if (contract.Status != ContractStatus.Approved)
            throw new InvalidOperationException("Invoice hanya bisa dibuat dari kontrak yang Approved.");
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(pdfUrl)) throw new ArgumentException("PDF URL wajib.", nameof(pdfUrl));

        return new Invoice
        {
            ContractId = contract.ContractId,
            Amount = amount,
            IssuedAt = issuedAtUtc,
            DueDate = dueDateUtc,
            PdfUrl = pdfUrl.Trim(),
            Status = InvoiceStatus.Draft
        };
    }

    public void SetNumber(string number)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Nomor hanya bisa di-set saat Draft.");
        if (!string.IsNullOrWhiteSpace(Number)) throw new InvalidOperationException("Nomor sudah terisi.");
        Number = number.Trim();
    }

    public void SetPdf(string pdfUrl)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("PDF hanya bisa di-set saat Draft.");
        if (string.IsNullOrWhiteSpace(pdfUrl)) throw new ArgumentException("PDF URL wajib.", nameof(pdfUrl));
        PdfUrl = pdfUrl.Trim();
    }

    public void Send()
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Hanya invoice Draft yang bisa dikirim.");
        if (string.IsNullOrWhiteSpace(Number)) throw new InvalidOperationException("Nomor invoice belum di-set.");
        if (string.IsNullOrWhiteSpace(PdfUrl)) throw new InvalidOperationException("PDF belum di-generate.");
        Status = InvoiceStatus.Sent;
    }

    public void MarkPaid(DateTime paidAtUtc)
    {
        if (Status != InvoiceStatus.Sent) throw new InvalidOperationException("Hanya invoice Sent yang bisa dibayar.");
        if (paidAtUtc < IssuedAt) throw new ArgumentException("PaidAt tidak boleh sebelum IssuedAt.", nameof(paidAtUtc));
        Status = InvoiceStatus.Paid;
        PaidAt = paidAtUtc;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid) throw new InvalidOperationException("Invoice yang sudah dibayar tidak bisa dibatalkan.");
        Status = InvoiceStatus.Cancelled;
    }
}
