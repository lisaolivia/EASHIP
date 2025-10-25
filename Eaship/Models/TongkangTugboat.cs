using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Models
{
    public class TongkangTugboat
    {
        public long TongkangId { get; set; }
        public long TugboatId { get; set; }

        // optional: navigasi (biar EF bisa include data)
        public Tongkang? Tongkang { get; set; }
        public Tugboat? Tugboat { get; set; }
    }
}
