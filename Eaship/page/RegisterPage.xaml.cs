using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Eaship.Services;
using Eaship.Models;

namespace Eaship.page
{
    public partial class RegisterPage : Page
    {
        private readonly IUserService _users;

        // Shortcut ke MainFrame
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RegisterPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
        }

        // ====== REGISTER BUTTON ======
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            var name = txtFullName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var pass = txtPassword.Password;
            var confirm = txtConfirm.Password;

            // Validasi input
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Semua field wajib diisi.", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Password tidak cocok.", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _users.RegisterAsync(name, email, pass);
                MessageBox.Show("Registrasi berhasil! Silakan login.", "Berhasil", MessageBoxButton.OK, MessageBoxImage.Information);
                Main?.Navigate(new LoginPage());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Gagal Registrasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal registrasi: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ====== BACK TO LOGIN ======
        private void GoLogin_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }

        // ====== NAVBAR: Sign Up ======
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kamu sudah di halaman Sign Up!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ====== NAVBAR: Log In ======
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
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

        // ====== NAVBAR: Help ======
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help diklik! (coming soon)");
        }
    }
}
