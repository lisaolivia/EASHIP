using Eaship.Models.nondb;
using Eaship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Helper
{
    public static class BookingMapper
    {
        public static BookingPreviewDTO ToPreview(Booking b)
        {
            return new BookingPreviewDTO
            {
                BookingId = b.BookingId,
                Route = $"{b.OriginPort} → {b.DestinationPort}",
                Cargo = b.CargoDesc,
                DateRange = $"{b.StartDate:dd MMM yyyy} - {b.GetEndDate():dd MMM yyyy}",
                StatusText = b.Status.ToString(),
                TotalTongkang = b.BookingTongkangs?.Count ?? 0
            };
        }
    }

}
