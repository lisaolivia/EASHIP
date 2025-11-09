using Eaship.Services;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page
{
    public partial class Pagelanding : Page
    {
        public Pagelanding()
        {
            InitializeComponent();
        }

        // Helper property for main frame navigation
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        // PROFILE (navbar -> Profile)
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            // TODO: Navigate to profile page
            MessageBox.Show("Profile clicked! (coming soon)");
        }

        // HELP (navbar -> Help)
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            // Help doesn't require login, just display info:
            MessageBox.Show("Help clicked! (coming soon)");
        }

        // INVOICE (navbar -> Invoice)
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            // TODO: Navigate to invoice page
            MessageBox.Show("Invoice clicked! (coming soon)");
        }

        // BARGES (navbar -> Barges)
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            // TODO: Navigate to barges list/booking page
            MessageBox.Show("Barges clicked! (stub)");
        }

        // MY BOOKINGS (navbar -> My Bookings)
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            // TODO: Navigate to user bookings page
            MessageBox.Show("My Bookings clicked! (stub)");
        }

        // LOG IN (navbar -> Login button)
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }

        // SIGN UP (navbar -> SignUp button)
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }

        // BOOK SHIP BUTTON (CTA -> Book Ship → button)
        // This now navigates to Login Page as requested
        private void BookShip_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }
    }
}