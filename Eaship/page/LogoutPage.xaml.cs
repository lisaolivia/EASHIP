using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eaship.page
{
    /// <summary>
    /// Interaction logic for LogoutPage.xaml
    /// </summary>
    public partial class LogoutPage : Page
    {
        private readonly IUserService _users;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public LogoutPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
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


        // ====== NAVBAR: Sign Up ======
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You are already Sign Up!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ====== NAVBAR: Log In ======
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You are already Log In!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
