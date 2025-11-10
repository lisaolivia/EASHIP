using Eaship.Models;
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
    }
}
