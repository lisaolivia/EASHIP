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
using Eaship.Models;
using Microsoft.EntityFrameworkCore;

namespace Eaship.page
{

    public partial class PICFormPage : Page
    {
        private readonly IUserService _users;
        private readonly EashipDbContext _context;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        public PICFormPage()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
            _context = App.Services.GetRequiredService<EashipDbContext>();
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




        private async void BtnSignup_company_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Session.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                return;
            }

            var data = Session.TempCompanyData;
            if (data == null)
            {
                MessageBox.Show("Data perusahaan belum diisi. Silakan kembali ke halaman sebelumnya.");
                return;
            }

            // Ambil data PIC
            var picName = TxtCompanyName.Text.Trim();
            var picPosition = TxtNPWP.Text.Trim();
            var picEmail = TxtCompanyAddress.Text.Trim();

            var renterCompany = new RenterCompany();
            renterCompany.SetCompanyInfo(
                data["Nama"],
                data["NPWP"],
                data["Address"],
                data["CityProvince"],
                picName,
                picPosition,
                picEmail,
                currentUser.UserId
            );

            typeof(RenterCompany).GetProperty("EmailBilling")?.SetValue(renterCompany, data["EmailBilling"]);
            typeof(RenterCompany).GetProperty("PhoneNumber")?.SetValue(renterCompany, data["PhoneNumber"]);

            _context.RenterCompanies.Add(renterCompany);
            await _context.SaveChangesAsync();

            MessageBox.Show("Company successfully registered! Please wait, admin will review your request.");
            Session.TempCompanyData = null;

            Main?.Navigate(new Dashboard());
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
