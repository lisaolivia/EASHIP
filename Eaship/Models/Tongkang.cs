namespace Eaship.Models;

public enum TongkangStatus
{
    Available,
    Assigned,
    Maintenance,
    Unavailable
}

public class Tongkang
{
    public int TongkangId { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int KapasitasDWT { get; set; }

    // true jika booking mensyaratkan tugboat
    public bool IncludeTugboat { get; private set; }

    
    public TongkangStatus Status { get; private set; } = TongkangStatus.Available;

    // ===== Relasi sederhana ke Tugboat (sementara pakai ID set) =====
    private readonly HashSet<int> _tugboatIds = new();
    public IReadOnlyCollection<int> TugboatIds => _tugboatIds;

    // ===== Methods =====
    public TongkangStatus CekStatus() => Status;

    /// <summary>
    /// Hitung harga sewa (contoh sederhana). Pakai decimal untuk uang.
    /// </summary>
    public decimal HitungHarga(string cargoDesc, int durasiHari)
    {
        if (durasiHari <= 0) throw new ArgumentOutOfRangeException(nameof(durasiHari));
        if (KapasitasDWT <= 0) throw new InvalidOperationException("KapasitasDWT belum di-set.");

        // TODO: ganti rumus sesuai kebutuhan bisnis
        decimal baseRatePerDwtPerDay = 100_000m;
        var harga = KapasitasDWT * durasiHari * baseRatePerDwtPerDay;

        // contoh: ada markup jika include tugboat
        if (IncludeTugboat) harga *= 1.10m;

        return harga;
    }

    public void AttachTugboat(Tugboat tug)
    {
        if (tug is null) throw new ArgumentNullException(nameof(tug));
        if (tug.CekStatus() == TugboatStatus.Maintenance)
            throw new InvalidOperationException("Tugboat sedang maintenance.");

        _tugboatIds.Add(tug.TugboatId);
        IncludeTugboat = true;
        Status = TongkangStatus.Assigned;

        tug.SetAssigned(); // method internal/protected di Tugboat
    }


    public void DetachTugboat(Tugboat tug)
    {
        if (tug is null) throw new ArgumentNullException(nameof(tug));
        _tugboatIds.Remove(tug.TugboatId);
        IncludeTugboat = _tugboatIds.Count > 0;
        if (_tugboatIds.Count == 0) Status = TongkangStatus.Available;
    }

    public void SendToMaintenance() => Status = TongkangStatus.Maintenance;
    public void MarkUnavailable() => Status = TongkangStatus.Unavailable;
    public void MarkAvailable() => Status = TongkangStatus.Available;
}
