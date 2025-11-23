using System.Windows;
using System.Windows.Controls;

namespace Eaship.page
{
    public partial class RequireLoginPage : Page
    {
        // Shortcut ke MainFrame di MainWindow
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RequireLoginPage()
        {
            InitializeComponent();
        }

        // ====== LOGIN BUTTON (di tengah & navbar) ======
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }

        // ====== SIGN UP BUTTON (navbar) ======
        private void BtnSignUpNav_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }

        // ============================
        // CENTER MENU → RequireLogin
        // ============================
        private void Brand_Click(object sender, RoutedEventArgs e)
        {

            NavigationService?.Navigate(new Pagelanding());
        }
        private void RequireLoginNotice()
        {
            MessageBox.Show(
                "You must log in first to access this feature.",
                "Login Required",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void BtnBarges_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

        private void BtnContract_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

        private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

        // CTA
        private void BookShip_Click(object sender, RoutedEventArgs e)
        {
            RequireLoginNotice();
            NavigationService?.Navigate(new RequireLoginPage());
        }

    }
}
