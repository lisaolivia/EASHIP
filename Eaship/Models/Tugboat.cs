using NpgsqlTypes;

namespace Eaship.Models;

public enum TugboatStatus
{
    AVAILABLE,
    ASSIGNED,
    MAINTENANCE
}

public class Tugboat
{
    public long TugboatId { get; set; } 
    public string Nama { get; set; } = string.Empty; // varchar(250)
    public string TugboatHp { get; set; } = string.Empty; // varchar(250)
    public TugboatStatus Status { get; set; } = TugboatStatus.AVAILABLE;
    public TugboatStatus CekStatus() => Status;
    public void SendToMaintenance() => Status = TugboatStatus.MAINTENANCE;

    public void ReleaseFromTongkang(Tongkang tongkang)
    {
        if (tongkang is null) throw new ArgumentNullException(nameof(tongkang));
        tongkang.DetachTugboat(this);
    }

    public void AssignToTongkang(Tongkang tongkang)
    {
        if (tongkang is null)
            throw new ArgumentNullException(nameof(tongkang));

        if (this.Status != TugboatStatus.AVAILABLE)
            throw new InvalidOperationException("Tugboat tidak tersedia untuk assignment.");

        tongkang.AttachTugboat(this);

        // WAJIB
        this.SetAssigned();
    }
    public void SetStatus(TugboatStatus status)
    {
        Status = status;
    }


    internal void SetAssigned() => Status = TugboatStatus.ASSIGNED;
    internal void SetAvailable() => Status = TugboatStatus.AVAILABLE;
}
