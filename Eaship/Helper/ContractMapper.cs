using Eaship.Models.nondb;
using Eaship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ContractMapper
{
    public static ContractPreviewDTO ToPreview(Contract c)
    {
        return new ContractPreviewDTO
        {
            ContractId = c.ContractId,
            CompanyName = c.Booking?.RenterCompany?.Nama ?? "Unknown",
            CompanyAddress = c.Booking?.RenterCompany?.Address ?? "-",
            CreatedAt = c.CreatedAt,
            Status = c.Status.ToString(),
            PdfUrl = c.PdfUrl
        };
    }
}

