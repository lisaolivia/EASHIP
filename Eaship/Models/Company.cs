namespace Eaship.Models;

public class Company
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NPWP { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    //BUAT AUDIT 
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }

    // Method
    private readonly List<Tongkang> _tongkangs = new();
    public IReadOnlyList<Tongkang> Tongkangs => _tongkangs;

    // ---- Invariants & behavior domain ----
    public void UpdateContact(string contact, string address)
    {
        Contact = contact ?? string.Empty;
        Address = address ?? string.Empty;
        UpdatedAt = DateTime.Now;
    }

    public Tongkang AddTongkang(string nama, int kapasitasDwt)
    {
        if (string.IsNullOrWhiteSpace(nama)) throw new ArgumentException("Nama tongkang wajib.");
        if (kapasitasDwt <= 0) throw new ArgumentOutOfRangeException(nameof(kapasitasDwt));

        var t = new Tongkang
        {
            CompanyId = CompanyId,
            Name = nama.Trim(),
            KapasitasDWT = kapasitasDwt,
            IncludeTugboat = false
            // Status default di Tongkang = Available (lihat enum di file Tongkang)
        };

        _tongkangs.Add(t);
        UpdatedAt = DateTime.Now;
        return t;
    }

    public bool RemoveTongkang(int tongkangId)
    {
        var idx = _tongkangs.FindIndex(x => x.TongkangId == tongkangId);
        if (idx < 0) return false;

        // Aturan bisnis contoh: hanya boleh hapus jika tidak Assigned
        if (_tongkangs[idx].CekStatus() != TongkangStatus.Available)
            throw new InvalidOperationException("Tongkang sedang tidak tersedia untuk dihapus.");

        _tongkangs.RemoveAt(idx);
        UpdatedAt = DateTime.Now;
        return true;
    }
}
