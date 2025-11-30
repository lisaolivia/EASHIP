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

    public string? PhotoUrl { get; set; }

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Nama tongkang tidak boleh kosong.")
            : value;
    }
    public string KapasitasDwt { get; set; } = string.Empty;

    public bool IncludeTugboat { get; set; }

    public TongkangStatus Status { get; private set; } = TongkangStatus.Available;

    private readonly HashSet<long> _tugboatIds = new();
    public IReadOnlyCollection<long> TugboatIds => _tugboatIds;

    public TongkangStatus CekStatus() => Status;

    //method


    public void SetStatus(TongkangStatus status)
    {
     
        if (Status == TongkangStatus.Assigned && status == TongkangStatus.Assigned)
            throw new InvalidOperationException("Tongkang sudah digunakan!");

        Status = status;
    }

    public string DisplayImageUrl =>
    string.IsNullOrWhiteSpace(PhotoUrl)
    ? "pack://application:,,,/Eaship;component/Assets/default_barge.jpg"
    : PhotoUrl;


    public void AttachTugboat(Tugboat tug)
    {
        if (tug is null) throw new ArgumentNullException(nameof(tug));

        if (this.Status != TongkangStatus.Available)
            throw new InvalidOperationException("Tongkang tidak tersedia untuk assignment.");

        if (tug.CekStatus() != TugboatStatus.AVAILABLE)
            throw new InvalidOperationException("Tugboat tidak tersedia untuk assignment.");

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
