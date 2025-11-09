using Eaship.Models;
using Microsoft.EntityFrameworkCore;

namespace Eaship.Services
{
    public interface IUserService
    {
        Task<bool> EmailExistsAsync(string email);
        Task<int> RegisterAsync(string fullName, string email, string password, string role, string phone);
        Task<Eaship.Models.User?> LoginAsync(string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly EashipDbContext _db;
        public UserService(EashipDbContext db) => _db = db;

        public Task<bool> EmailExistsAsync(string email)
            => _db.Set<Eaship.Models.User>().AnyAsync(u => u.Email == email);

        public async Task<int> RegisterAsync(string fullName, string email, string password, string role, string phone)
        {
            if (await EmailExistsAsync(email))
                throw new InvalidOperationException("Email sudah terdaftar.");

            var user = new Eaship.Models.User();
            user.FullName = fullName;
            user.Email = email;
            user.Register(password);

            // convert string role ke enum
            if (Enum.TryParse<UserRole>(role, out var parsedRole))
                user.Role = parsedRole;
            else
                user.Role = UserRole.Renter; // default fallback

          
            user.Phone = phone;

            _db.Set<User>().Add(user);
            await _db.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<Eaship.Models.User?> LoginAsync(string email, string password)
        {
            var user = await _db.Set<Eaship.Models.User>()
                                .SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return null;

            if (!user.Login(password)) return null;  // verify + set LastLoginAt
            await _db.SaveChangesAsync();            // simpan LastLoginAt
            return user;                             // Login => kembalikan User
        }
    }
    public static class Session
    {
        private static Models.User? _currentUser;

        public static Models.User? CurrentUser => _currentUser;
        public static bool IsLoggedIn => _currentUser != null;

        public static void Set(Models.User user)
        {
            _currentUser = user;
        }

        public static void Clear()
        {
            _currentUser = null;
        }
    }
}
