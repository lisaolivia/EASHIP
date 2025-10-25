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

        // helper biar singkat
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        // PROFILE (navbar -> Profile)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            // TODO: ganti ke halaman profile kamu
            MessageBox.Show("Profile diklik! (coming soon)");
        }

        // HELP (navbar -> Help) atau INVOICE kalau kamu mapping ke sini
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // kalau Help ga perlu login, tinggal tampilkan info:
            MessageBox.Show("Help diklik! (coming soon)");
            // Jika kamu pakai ini untuk Invoice yang butuh login, pakai blok berikut:
            // if (!Session.IsLoggedIn) { Main?.Navigate(new RequireLoginPage()); return; }
            // MessageBox.Show("Invoice diklik! (coming soon)");
        }

        // BARGES (navbar -> Barges)
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            // TODO: ganti ke halaman daftar barges / booking
            MessageBox.Show("Barges diklik! (stub sementara)");
        }

        // MY BOOKINGS (navbar -> My Bookings)
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            // TODO: ganti ke halaman bookings user
            MessageBox.Show("My Bookings diklik! (stub sementara)");
        }

        // LOG IN (navbar -> Log In)
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }

        // SIGN UP (navbar -> Sign Up)
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }
    }
}
