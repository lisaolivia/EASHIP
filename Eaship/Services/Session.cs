using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Eaship.Services
{


    public static class Session
    {
        public static bool IsLoggedIn { get; private set; }
        public static User? CurrentUser { get; private set; }

        public static Dictionary<string, string>? TempCompanyData { get; set; }


        public static void Set(User user)
        {
            CurrentUser = user;
            IsLoggedIn = true;
        }

        public static void Clear()
        {
            CurrentUser = null;
            IsLoggedIn = false;
        }

        public static async Task RefreshAsync(EashipDbContext db)
        {
            if (CurrentUser != null)
            {
                CurrentUser = await db.Users.FirstAsync(u => u.UserId == CurrentUser.UserId);
            }
        }

    }
}
