using Eaship.Models;
using Eaship.page.Renter;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
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

        var picName = TxtPICName.Text.Trim();
        var picPosition = TxtPICPosition.Text.Trim();
        var picEmail = TxtPICEmail.Text.Trim();

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

        renterCompany.EmailBilling = data["EmailBilling"];
        renterCompany.PhoneNumber = data["PhoneNumber"];

        _context.RenterCompanies.Add(renterCompany);
        await _context.SaveChangesAsync();

        // SET FK user → company
        currentUser.RenterCompanyId = renterCompany.RenterCompanyId;
        await _context.SaveChangesAsync();
        await Session.RefreshAsync(_context);


            // ⭐ REFRESH SESSION USER DENGAN CARA BENAR
            var freshUser = await _context.Users
            .FirstAsync(u => u.UserId == currentUser.UserId);

        Session.Set(freshUser); // <-- WAJIB pakai ini, bukan assign langsung

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

        // ==========================================================
        //                     NAVBAR HANDLERS
        // ==========================================================
        private void BtnBarges_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new Barges());
        }

        private void BtnMyBookings_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new MyBookingPage());
        }

        private void BtnContract_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ContractPage()); // ganti kalau nama lain
        }

        private void BtnNotif_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new NotificationPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ProfilPage()); // ganti sesuai nama kamu
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }



    }
}
