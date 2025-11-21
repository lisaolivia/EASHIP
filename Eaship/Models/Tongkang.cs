using NpgsqlTypes;

namespace Eaship.Models;

public enum TongkangStatus
{
    [PgName("AVAILABLE")] Available,
    [PgName("ASSIGNED")] Assigned,
    [PgName("MAINTENANCE")] Maintenance,
    [PgName("UNAVAILABLE")] Unavailable
}

public class Tongkang
{
    public long TongkangId { get; set; }
   
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Nama tongkang tidak boleh kosong.")
            : value;
    }
    public string KapasitasDwt { get; set; } = string.Empty;
  



    public bool IncludeTugboat { get; private set; }
    public TongkangStatus Status { get; private set; } = TongkangStatus.Available;



    private readonly HashSet<long> _tugboatIds = new();
    public IReadOnlyCollection<long> TugboatIds => _tugboatIds;

    public TongkangStatus CekStatus() => Status;

    //method
    public void SetStatus(TongkangStatus status)
    {
        Status = status;
    }

    public decimal HitungHarga(string cargoDesc, int durasiHari)
    {
        if (durasiHari <= 0) throw new ArgumentOutOfRangeException(nameof(durasiHari));
        // jika butuh angka dari KapasitasDwt, parse dulu (mis. "8000 DWT" → 8000)
        // sementara abaikan untuk contoh singkat
        decimal baseRate = 100_000m;
        var kapasitas = 1m; // TODO: parse dari KapasitasDwt
        var harga = kapasitas * durasiHari * baseRate;
        if (IncludeTugboat) harga *= 1.10m;
        return harga;
    }

    public void AttachTugboat(Tugboat tug)
    {
        if (tug is null) throw new ArgumentNullException(nameof(tug));
        if (tug.CekStatus() == TugboatStatus.MAINTENANCE)
            throw new InvalidOperationException("Tugboat sedang maintenance.");

        _tugboatIds.Add(tug.TugboatId);
        IncludeTugboat = true;
        Status = TongkangStatus.Assigned;
        tug.SetAssigned();
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
