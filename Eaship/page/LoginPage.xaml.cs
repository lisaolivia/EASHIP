using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Eaship.page
{
    public partial class LoginPage : Page
    {
        private readonly IUserService _users;

        // Shortcut ke MainFrame biar singkat
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public LoginPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
        }

        // ====== LOGIN BUTTON ======
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var pass = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Email dan password wajib diisi.", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = await _users.LoginAsync(email, pass);
                if (user == null)
                {
                    MessageBox.Show("Email atau password salah.", "Gagal Login", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Simpan user di Session
                Session.Set(user);
                MessageBox.Show($"Selamat datang, {user.FullName}!", "Login Berhasil", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigasi ke landing page
                Main?.Navigate(new Dashboard());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ====== SIGN UP BUTTON (di bawah form) ======
        private void GoRegister_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }

        // ====== NAVBAR: Barges ======
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            MessageBox.Show("Barges diklik! (stub sementara)");
        }

        // ====== NAVBAR: My Bookings ======
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            MessageBox.Show("My Bookings diklik! (stub sementara)");
        }


        // ====== NAVBAR: SignUp ======
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RegisterPage());
        }


        private void txtForgetPassword_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Forgot Password diklik! (coming soon)");
        }

        private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
