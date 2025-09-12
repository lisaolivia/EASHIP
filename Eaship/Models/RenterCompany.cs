public enum CompanyStatus { Draft, Validating, Active, Rejected }

public class RenterCompany
{
    public int RenterCompanyId { get; private set; }
    public string Nama { get; private set; } = string.Empty;
    public string NPWP { get; private set; } = string.Empty;
    public string Contact { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    public CompanyStatus Status { get; private set; } = CompanyStatus.Draft;

    public void SubmitForValidation()
    {
        if (string.IsNullOrWhiteSpace(Nama) ||
            string.IsNullOrWhiteSpace(NPWP) ||
            string.IsNullOrWhiteSpace(Contact) ||
            string.IsNullOrWhiteSpace(Address))
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
            throw new InvalidOperationException("Perusahaan belum masuk status Validating.");

        Status = CompanyStatus.Active;
    }

    public void Reject(User admin)
    {
        if (admin.Role != UserRole.Admin)
            throw new UnauthorizedAccessException("Hanya admin yang bisa reject.");

        if (Status != CompanyStatus.Validating)
            throw new InvalidOperationException("Perusahaan belum masuk status Validating.");

        Status = CompanyStatus.Rejected;
    }
}
