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
    // Booking.cs
    public class Booking
    {
        public long BookingId { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }

        public string OriginPort { get; set; } = string.Empty;

        public int? RenterCompanyId { get; set; }
        public RenterCompany? RenterCompany { get; set; }

        public string DestinationPort { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public string CargoDesc { get; set; } = string.Empty;

        public BookingStatus Status { get; private set; } = BookingStatus.Requested;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // NEW: navigasi M-N
        public ICollection<BookingTongkang> BookingTongkangs { get; set; } = new List<BookingTongkang>();


        public DateTime GetEndDate() => StartDate.Date.AddDays(DurationDays);


        public bool VerifikasiBooking()
        {
            return !string.IsNullOrWhiteSpace(OriginPort)
                && !string.IsNullOrWhiteSpace(DestinationPort)
                && DurationDays > 0
                && StartDate.Date >= DateTime.UtcNow.Date
                && BookingTongkangs?.Count > 0; 
        }


        public void Confirm(IEnumerable<(Tongkang tongkang, int days)> itemsPerTongkang)
        {
            if (!VerifikasiBooking())
                throw new InvalidOperationException("Booking belum valid.");

            var totalDays = itemsPerTongkang.Sum(x => x.days);
            if (totalDays != DurationDays)
                throw new InvalidOperationException("Total alokasi hari per tongkang tidak sama dengan DurationDays.");

            Status = BookingStatus.Confirmed;
        }

        public void Approve()
        {
            if (Status != BookingStatus.Requested)
                throw new InvalidOperationException("Hanya booking Requested yang bisa di-approve.");

            Status = BookingStatus.Confirmed;
        }

        public void Decline()
        {
            if (Status != BookingStatus.Requested)
                throw new InvalidOperationException("Hanya booking Requested yang bisa di-decline.");

            Status = BookingStatus.Cancelled;
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

