using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page
{
    public partial class LogoutPage : Page
    {
        private readonly IUserService _users;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public LogoutPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();

            // Cegah orang buka halaman ini tanpa login
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new LoginPage());
                return;
            }

            // Hubungkan event handler
            BtnCancel.Click += BtnCancel_Click;
            BtnYes.Click += BtnYes_Click;
        }

        // === UNIVERSAL LOGOUT ===
        private void DoLogout()
        {
            // 1. Clear session
            Session.Logout();

            // 2. Arahkan ke login page
            Main?.Navigate(new LoginPage());

            // 3. Buang history biar ga bisa Back
            while (Main != null && Main.CanGoBack)
                Main.RemoveBackEntry();
        }

        // === CANCEL button (kembali ke halaman sebelumnya) ===
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Main?.GoBack();
        }

        // === YES button (logout) ===
        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            DoLogout();
        }

    }
}
