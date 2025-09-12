namespace Eaship.Models;

public enum InvoiceStatus
{
    Draft,
    Sent,
    Paid,
    Cancelled
}

public class Invoice
{
    public int InvoiceId { get; set; }
    public int ContractId { get; set; }
    public string Number { get; set; } = string.Empty;  // contoh: INV-2025-001
    public decimal Amount { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.Now;
    public DateTime? PaidAt { get; set; }
    
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;

    public static Invoice CreateFromContract(Contract contract, decimal amount, string number, DateTime issuedAtUtc, DateTime? dueDateUtc, string pdfUrl)
    {
        if (contract is null) throw new ArgumentNullException(nameof(contract));
        if (contract.Status != ContractStatus.Approved)
            throw new InvalidOperationException("Invoice hanya bisa dibuat dari kontrak yang Approved.");
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(number)) throw new ArgumentException("Nomor invoice wajib.");
        if (string.IsNullOrWhiteSpace(pdfUrl)) throw new ArgumentException("PDF URL wajib.");

        return new Invoice
        {
            ContractId = contract.ContractId,
            Amount = amount,
            Number = number.Trim(),
            IssuedAt = issuedAtUtc,
            DueDate = dueDateUtc,
            PdfUrl = pdfUrl.Trim(),
            Status = InvoiceStatus.Draft
        };
    }

    // ----- Actions -----
    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Hanya invoice Draft yang bisa dikirim.");
        if (string.IsNullOrWhiteSpace(Number))
            throw new InvalidOperationException("Nomor invoice belum di-set.");
        if (string.IsNullOrWhiteSpace(PdfUrl))
            throw new InvalidOperationException("PDF belum di-generate.");

        Status = InvoiceStatus.Sent;
    }

    public void MarkPaid(DateTime paidAtUtc)
    {
        if (Status != InvoiceStatus.Sent)
            throw new InvalidOperationException("Hanya invoice Sent yang bisa dibayar.");
        Status = InvoiceStatus.Paid;
        PaidAt = paidAtUtc;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Invoice yang sudah dibayar tidak bisa dibatalkan.");
        Status = InvoiceStatus.Cancelled;
    }

    // (Opsional) setter terkontrol
    public void SetPdf(string pdfUrl)
    {
        if (string.IsNullOrWhiteSpace(pdfUrl)) throw new ArgumentException("PDF URL wajib.");
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("PDF hanya bisa di-set saat Draft.");
        PdfUrl = pdfUrl.Trim();
    }
}
