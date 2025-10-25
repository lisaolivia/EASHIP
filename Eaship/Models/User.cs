using System.Security.Cryptography;

namespace Eaship.Models;

public enum UserRole
{
    Renter,
    Admin
}

public class User
{
    public int UserId { get; set; }
    public int? RenterCompanyId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Renter;

    // password aman (hash + salt)
    public byte[] PasswordHash { get; private set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; private set; } = Array.Empty<byte>();


    // waktu terakhir login
    public DateTime? LastLoginAt { get; private set; }

    // ===== Methods penting =====

    // dipakai saat daftar
    public void Register(string rawPassword)
    {
        (PasswordHash, PasswordSalt) = HashPassword(rawPassword);
    }

    // dipakai saat login
    public bool Login(string rawPassword)
    {
        if (!VerifyPassword(rawPassword))
            return false;

        LastLoginAt = DateTime.UtcNow;
        return true;
    }

    // dipakai kalau mau ganti password
    public void ChangePassword(string newPassword)
    {
        (PasswordHash, PasswordSalt) = HashPassword(newPassword);
    }

    // ===== Helper hashing =====
    private static (byte[] hash, byte[] salt) HashPassword(string raw)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(raw, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        return (hash, salt);
    }

    private bool VerifyPassword(string raw)
    {
        if (PasswordHash.Length == 0 || PasswordSalt.Length == 0) return false;

        using var pbkdf2 = new Rfc2898DeriveBytes(raw, PasswordSalt, 100_000, HashAlgorithmName.SHA256);
        var candidate = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(candidate, PasswordHash);
    }
}
