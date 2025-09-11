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
    public string Port { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;  // contoh: INV-2025-001
    public decimal Amount { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.Now;
    public DateTime? PaidAt { get; set; }
    public string PdfUrl { get; set; } = string.Empty;
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;

    // ===== Methods =====
    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Hanya invoice Draft yang bisa dikirim.");
        Status = InvoiceStatus.Sent;
    }

    public void MarkPaid()
    {
        if (Status != InvoiceStatus.Sent)
            throw new InvalidOperationException("Hanya invoice Sent yang bisa dibayar.");
        Status = InvoiceStatus.Paid;
        PaidAt = DateTime.Now;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Invoice yang sudah dibayar tidak bisa dibatalkan.");
        Status = InvoiceStatus.Cancelled;
    }

    public void GeneratePDF()
    {
        // TODO: bikin logic generate PDF dan update PdfUrl
    }
}
