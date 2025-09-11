namespace Eaship.Models;


public enum BookingStatus
{
    REQUESTED,
    CONFIRMED,
    IN_PROGRESS,
    COMPLETED,
    CANCELLED
}

public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public string OriginPort { get; set; } = string.Empty;
    public string DestinationPort { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int Durationday { get; set; }
    public string CargoDesc { get; set; } = string.Empty;
    public decimal HargaTotal { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.REQUESTED;

    // Method
    public decimal HitungTotalHarga()
    {
        return Durationday * 1000000; // contoh perhitungan
    }

    public bool VerifikasiBooking()
    {
        return !string.IsNullOrEmpty(OriginPort) && !string.IsNullOrEmpty(DestinationPort);
    }
}
