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
            // Get selected role from ComboBox
            var selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var name = txtFullName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var phone = txtPhone.Text.Trim();
            var pass = txtPassword.Password;
            var confirm = txtConfirm.Password;

            // Input validation
            if (string.IsNullOrWhiteSpace(selectedRole) ||
                string.IsNullOrWhiteSpace(name) ||
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
                // Register with the service (you may need to update IUserService to accept role and phone)
                // For now, we're just passing name, email, and password
                await _users.RegisterAsync(name, email, pass);

                // TODO: If you want to store role and phone, update your IUserService.RegisterAsync method
                // Example: await _users.RegisterAsync(name, email, pass, selectedRole, phone);

                MessageBox.Show($"Registration successful as {selectedRole}! Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Main?.Navigate(new LoginPage());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        // ====== NAVBAR: Barges ======
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            MessageBox.Show("Barges clicked! (stub)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ====== NAVBAR: My Bookings ======
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }
            MessageBox.Show("My Bookings clicked! (stub)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ====== NAVBAR: Help ======
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help clicked! (coming soon)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}