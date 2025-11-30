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
        public int? DaysAllocated { get; set; }     
        public int? SequenceNo { get; set; }        

        public Booking? Booking { get; set; }
        public Tongkang? Tongkang { get; set; }
    }
}
