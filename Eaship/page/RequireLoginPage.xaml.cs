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
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }

        // ====== NAVBAR: Barges ======
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Silakan login terlebih dahulu untuk melihat daftar barges.",
                            "Login Diperlukan",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        // ====== NAVBAR: My Bookings ======
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Silakan login terlebih dahulu untuk melihat booking Anda.",
                            "Login Diperlukan",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }




        // ====== NAVBAR: Profile ======
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Silakan login terlebih dahulu untuk membuka profil Anda.",
                            "Login Diperlukan",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
