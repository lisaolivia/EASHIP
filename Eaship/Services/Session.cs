using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaship.Services
{
    public static class Session
    {
        public static bool IsLoggedIn { get; private set; }
        public static User? CurrentUser { get; private set; }

        // data sementara saat mendaftar perusahaan
        public static Dictionary<string, string>? TempCompanyData { get; set; }

        // Login
        public static void Set(User user)
        {
            CurrentUser = user;
            IsLoggedIn = true;
        }

        // Clear session
        public static void Clear()
        {
            CurrentUser = null;
            IsLoggedIn = false;
            TempCompanyData = null;
        }

        // Refresh user info from DB (optional)
        public static async Task RefreshAsync(EashipDbContext db)
        {
            if (CurrentUser != null)
            {
                CurrentUser = await db.Users.FirstAsync(u => u.UserId == CurrentUser.UserId);
            }
        }

        // LOGOUT FINAL FIX
        public static void Logout()
        {
            CurrentUser = null;
            IsLoggedIn = false;     // <<< INI DULU GAK ADA
            TempCompanyData = null; // opsional reset data
        }
    }
}
