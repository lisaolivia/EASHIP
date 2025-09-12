namespace Eaship.Models;

public enum TugboatStatus
{
    Available,
    Assigned,
    Maintenance
}

public class Tugboat
{
    public int TugboatId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TugboatHP { get; set; }

    public TugboatStatus Status { get; private set; } = TugboatStatus.Available;

    public TugboatStatus CekStatus() => Status;

    public void SendToMaintenance()
    {
        Status = TugboatStatus.Maintenance;
    }

    //  Delegasi saja, TANPA set status di sini.
    public void AssignToTongkang(Tongkang tongkang)
    {
        if (tongkang is null) throw new ArgumentNullException(nameof(tongkang));
        tongkang.AttachTugboat(this); // biarkan Tongkang yang atur semua state
    }

    public void ReleaseFromTongkang(Tongkang tongkang)
    {
        if (tongkang is null) throw new ArgumentNullException(nameof(tongkang));
        tongkang.DetachTugboat(this); // biarkan Tongkang yang atur semua state
    }

    // HANYA untuk dipanggil dari Tongkang
    internal void SetAssigned() => Status = TugboatStatus.Assigned;
    internal void SetAvailable() => Status = TugboatStatus.Available;
}
