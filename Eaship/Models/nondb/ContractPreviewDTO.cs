using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Models.nondb
{
    public class ContractPreviewDTO
    {
        public long ContractId { get; set; }

        public string CompanyName { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PdfUrl { get; set; }
    }

}
