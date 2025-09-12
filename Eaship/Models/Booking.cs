namespace Eaship.Models;

public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }

    public int TongkangId { get; set; }               
    public string OriginPort { get; set; } = string.Empty;
    public string DestinationPort { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public int DurationDays { get; set; }             
    public string CargoDesc { get; set; } = string.Empty;

    public decimal HargaTotal { get; private set; }    // <- tetapkan saat confirm
    public BookingStatus Status { get; private set; } = BookingStatus.REQUESTED;

    public DateTime GetEndDate() => StartDate.Date.AddDays(DurationDays);

    public bool VerifikasiBooking()
    {
        return !string.IsNullOrWhiteSpace(OriginPort)
            && !string.IsNullOrWhiteSpace(DestinationPort)
            && DurationDays > 0
            && StartDate.Date >= DateTime.UtcNow.Date
            && TongkangId > 0;
    }

    // Hitung harga konsisten dengan Tongkang
    public decimal HitungTotalHarga(Tongkang tongkang)
    {
        if (tongkang is null) throw new ArgumentNullException(nameof(tongkang));
        if (tongkang.TongkangId != TongkangId) throw new InvalidOperationException("Tongkang tidak cocok.");
        return tongkang.HitungHarga(CargoDesc, DurationDays);
    }

    // State transitions yang menetapkan harga
    public void Confirm(Tongkang tongkang)
    {
        if (!VerifikasiBooking()) throw new InvalidOperationException("Booking belum valid.");
        HargaTotal = HitungTotalHarga(tongkang); // lock harga saat confirm
        Status = BookingStatus.CONFIRMED;
    }

    public void verifyCompletion(DateTime actualEndDate)
    {
        if (Status != BookingStatus.IN_PROGRESS) throw new InvalidOperationException("Booking belum mulai.");
        if (actualEndDate.Date < GetEndDate()) throw new InvalidOperationException("Tanggal selesai tidak valid.");
        Status = BookingStatus.COMPLETED;
    }

    public void Start() { if (Status != BookingStatus.CONFIRMED) throw; Status = BookingStatus.IN_PROGRESS; }
    public void Complete() { if (Status != BookingStatus.IN_PROGRESS) throw; Status = BookingStatus.COMPLETED; }
    public void Cancel() { if (Status == BookingStatus.COMPLETED) throw; Status = BookingStatus.CANCELLED; }
}
