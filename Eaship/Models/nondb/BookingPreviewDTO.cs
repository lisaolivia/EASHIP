using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Models.nondb
{
    public class BookingPreviewDTO
    {
        public long BookingId { get; set; }
        public string Route { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string DateRange { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;
        public int TotalTongkang { get; set; }
    }

}
