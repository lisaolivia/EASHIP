using System;

namespace Eaship.Models
{
    // Enum BookingStatus dulu
    public enum BookingStatus
    {
        Requested,
        Confirmed,
        InProgress,
        Completed,
        Cancelled
    }

    // Baru class Booking
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

        public decimal HargaTotal { get; private set; }
        public BookingStatus Status { get; private set; } = BookingStatus.Requested;

        public DateTime GetEndDate() => StartDate.Date.AddDays(DurationDays);

        public bool VerifikasiBooking()
        {
            return !string.IsNullOrWhiteSpace(OriginPort)
                && !string.IsNullOrWhiteSpace(DestinationPort)
                && DurationDays > 0
                && StartDate.Date >= DateTime.UtcNow.Date
                && TongkangId > 0;
        }

        public decimal HitungTotalHarga(Tongkang tongkang)
        {
            if (tongkang is null) throw new ArgumentNullException(nameof(tongkang));
            if (tongkang.TongkangId != TongkangId)
                throw new InvalidOperationException("Tongkang tidak cocok.");
            return tongkang.HitungHarga(CargoDesc, DurationDays);
        }

        public void Confirm(Tongkang tongkang)
        {
            if (!VerifikasiBooking())
                throw new InvalidOperationException("Booking belum valid.");
            HargaTotal = HitungTotalHarga(tongkang);
            Status = BookingStatus.Confirmed;
        }

        public void VerifyCompletion(DateTime actualEndDate)
        {
            if (Status != BookingStatus.InProgress)
                throw new InvalidOperationException("Booking belum mulai.");
            if (actualEndDate.Date < GetEndDate())
                throw new InvalidOperationException("Tanggal selesai tidak valid.");
            Status = BookingStatus.Completed;
        }

        public void Start()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Booking belum dikonfirmasi.");
            Status = BookingStatus.InProgress;
        }

        public void Complete()
        {
            if (Status != BookingStatus.InProgress)
                throw new InvalidOperationException("Booking belum berjalan.");
            Status = BookingStatus.Completed;
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Completed)
                throw new InvalidOperationException("Booking sudah selesai.");
            Status = BookingStatus.Cancelled;
        }
    }
}
