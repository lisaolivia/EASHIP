using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Eaship.page
    {
        public partial class Pagelanding : Page
        {
            public Pagelanding()
            {
                InitializeComponent();
            }

            // BRAND CLICK → ke Landing lagi
            private void Brand_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new Pagelanding());
            }

            // SIGN UP → ke Register Page
            private void SignUp_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RegisterPage());
            }

            // LOGIN → ke LoginPage
            private void Login_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new LoginPage());
            }

            // ============================
            // CENTER MENU → RequireLogin
            // ============================

            private void BtnBarges_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }

            private void BtnBookings_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }

            private void BtnContract_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }

            private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }

            private void BtnProfile_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }

            // CTA: Book Ship →
            private void BookShip_Click(object sender, RoutedEventArgs e)
            {
                NavigationService?.Navigate(new RequireLoginPage());
            }
        }
    }


