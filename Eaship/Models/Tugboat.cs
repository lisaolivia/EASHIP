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

    // Status kapal tunda saat ini
    public TugboatStatus Status { get; private set; } = TugboatStatus.Available;

    // Baca status (boleh juga cukup pakai properti Status tanpa method ini)
    public TugboatStatus CekStatus() => Status;

    // Kirim ke perawatan
    public void SendToMaintenance()
    {
        Status = TugboatStatus.Maintenance;
    }

    // Assign ke tongkang (akan set status -> Assigned)
    public void AssignToTongkang(Tongkang tongkang)
    {
        if (Status == TugboatStatus.Maintenance)
            throw new InvalidOperationException("Tugboat sedang maintenance.");

        Status = TugboatStatus.Assigned;
        // Opsional: beritahu Tongkang untuk mencatat relasi
        tongkang.AttachTugboat(this); // pastikan method ini ada di class Tongkang
    }

    // Lepas dari tongkang (kembali Available)
    public void ReleaseFromTongkang(Tongkang tongkang)
    {
        tongkang.DetachTugboat(this); // siapkan method ini di Tongkang
        Status = TugboatStatus.Available;
    }
}
