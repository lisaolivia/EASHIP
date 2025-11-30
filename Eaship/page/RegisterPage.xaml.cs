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

        // Shortcut to MainFrame
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RegisterPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
        }

        // ====== REGISTER BUTTON ======
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
          
            var role = "RENTER";

            var name = txtFullName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var phone = txtPhone.Text.Trim();
            var pass = txtPassword.Password;
            var confirm = txtConfirm.Password;

            // Input validation
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("All fields are required.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Register dengan role yang sudah dikunci
                await _users.RegisterAsync(name, email, pass, role, phone);

                MessageBox.Show($"Registration successful as {role}! Please login.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Main?.Navigate(new LoginPage());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Brand_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService?.Navigate(new Pagelanding());
        }

        // ====== NAVBAR: Sign Up ======
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You are already on the Sign Up page!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ====== NAVBAR: Log In ======
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new LoginPage());
        }

    }
}