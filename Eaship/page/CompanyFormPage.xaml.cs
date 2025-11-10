using Eaship.Services;
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
using Microsoft.Extensions.DependencyInjection;
using Eaship.Models;

namespace Eaship.page
{
    /// <summary>
    /// Interaction logic for CompanyFormPage.xaml
    /// </summary>
    public partial class CompanyFormPage : Page
    {
        private readonly IUserService _users;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        public CompanyFormPage()
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

        // ====== NAVBAR: Help ======
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help diklik! (coming soon)");
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

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Session.TempCompanyData = new Dictionary<string, string>
            {
                ["Nama"] = TxtCompanyName.Text.Trim(),
                ["NPWP"] = TxtNPWP.Text.Trim(),
                ["Address"] = TxtCompanyAddress.Text.Trim(),
                ["CityProvince"] = TxtCityProvince.Text.Trim(),
                ["EmailBilling"] = TxtEmailBilling.Text.Trim(),
                ["PhoneNumber"] = TxtPhoneNumber.Text.Trim()
            };

            Main?.Navigate(new PICFormPage());
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                MessageBox.Show("Tidak ada halaman sebelumnya.");
        }


    }
}
