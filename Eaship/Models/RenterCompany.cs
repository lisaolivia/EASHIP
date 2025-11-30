namespace Eaship.Models;

public enum CompanyStatus { Draft, Validating, Active, Rejected }

public class RenterCompany
{
    public int RenterCompanyId { get; set; }

    // COMPANY INFO
    public string Nama { get; set; } = string.Empty;
    public string NPWP { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? CityProvince { get; set; }
    public string JoinCode { get; set; } = GenerateJoinCode();
    public string EmailBilling { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }

    // PIC INFO
    public string PICName { get; set; } = string.Empty;
    public string PICPosition { get; set; } = string.Empty;
    public string PICEmail { get; set; } = string.Empty;

    public CompanyStatus Status { get; set; } = CompanyStatus.Validating;
    public long CreatedBy { get; set; }
    public string? RejectedReason { get; set; }

    // --- METHODS ---
    public void SubmitForValidation()
    {
        if (string.IsNullOrWhiteSpace(Nama) ||
            string.IsNullOrWhiteSpace(NPWP) ||
            string.IsNullOrWhiteSpace(Address) ||
            string.IsNullOrWhiteSpace(PICName))
        {
            throw new InvalidOperationException("Data perusahaan belum lengkap.");
        }
        Status = CompanyStatus.Validating;
    }

    public void Approve(User admin)
    {
        if (admin.Role != UserRole.Admin)
            throw new UnauthorizedAccessException("Hanya admin yang bisa approve.");
        if (Status != CompanyStatus.Validating)
            throw new InvalidOperationException("Perusahaan belum dalam status Validating.");
        Status = CompanyStatus.Active;
    }

    public void Reject(User admin)
    {
        if (admin.Role != UserRole.Admin)
            throw new UnauthorizedAccessException("Hanya admin yang bisa reject.");
        if (Status != CompanyStatus.Validating)
            throw new InvalidOperationException("Perusahaan belum dalam status Validating.");
        Status = CompanyStatus.Rejected;
    }
    public static string GenerateJoinCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }

    public void SetCompanyInfo(string nama, string npwp, string address, string cityProvince,
                           string picName, string picPosition, string picEmail, long createdBy)
    {
        Nama = nama;
        NPWP = npwp;
        Address = address;
        CityProvince = cityProvince;
        PICName = picName;
        PICPosition = picPosition;
        PICEmail = picEmail;
        CreatedBy = createdBy;
        Status = CompanyStatus.Validating;
    }

}
