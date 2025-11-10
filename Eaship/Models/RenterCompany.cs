namespace Eaship.Models;

public enum CompanyStatus { Draft, Validating, Active, Rejected }

public class RenterCompany
{
    public int RenterCompanyId { get; private set; }

    // COMPANY INFO
    public string Nama { get; private set; } = string.Empty;
    public string NPWP { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string? CityProvince { get; private set; }

    public string EmailBilling { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    // PIC INFO
    public string PICName { get; private set; } = string.Empty;
    public string PICPosition { get; private set; } = string.Empty;
    public string PICEmail { get; private set; } = string.Empty;

    public CompanyStatus Status { get; private set; } = CompanyStatus.Draft;
    public long CreatedBy { get; private set; } // User ID pembuat (auto dari sesi login)

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
