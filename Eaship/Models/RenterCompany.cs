namespace Eaship.Models;

public class RenterCompany
{
    public int RenterCompanyId { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string NPWP { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Method


    public string GetCompanyDetails()
    {
        return $"ID: {RenterCompanyId}, Nama: {Nama}, NPWP: {NPWP}, Contact: {Contact}, Address: {Address}";
    }

    public void RegisterCompany(string nama, string npwp, string contact, string address)
    {
        ValidateCompany(nama, npwp, contact, address);

        var clean = new string(npwp.Where(char.IsLetterOrDigit).ToArray());
        Nama = nama.Trim();
        NPWP = clean;
        Contact = contact.Trim();
        Address = address.Trim();
       
    }

    private void ValidateCompany(string nama, string npwp, string contact, string address)
    {
        if (string.IsNullOrWhiteSpace(nama)) throw new ArgumentException("Nama perusahaan wajib.");
        if (string.IsNullOrWhiteSpace(npwp)) throw new ArgumentException("NPWP wajib.");
        if (string.IsNullOrWhiteSpace(contact)) throw new ArgumentException("Kontak wajib.");
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Alamat wajib.");

        var clean = new string(npwp.Where(char.IsLetterOrDigit).ToArray());
        if (clean.Length < 15) throw new ArgumentException("Format NPWP tidak valid.");
    }
}
