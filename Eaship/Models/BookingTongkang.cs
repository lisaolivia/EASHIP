using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Models
{
    public class BookingTongkang
    {
        public long BookingId { get; set; }
        public long TongkangId { get; set; }
        public int? DaysAllocated { get; set; }     // alokasi hari untuk tongkang ini
        public int? SequenceNo { get; set; }        // urutan pemakaian (kalau berantai)

        // navigasi opsional
        public Booking? Booking { get; set; }
        public Tongkang? Tongkang { get; set; }
    }
}
