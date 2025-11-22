using Eaship.Models;
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


namespace Eaship.page.Renter
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private readonly IUserService _users;
        private readonly EashipDbContext _context;
        private User? _currentUser;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        // untuk simpan user aktif

        public Dashboard()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
            _context = App.Services.GetRequiredService<EashipDbContext>(); // ambil user aktif dari session
            Loaded += Dashboard_Loaded;
            _currentUser = Session.CurrentUser;
            
        }


        private void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                _currentUser = Session.CurrentUser;
                if (_currentUser == null)
                {
                    MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                    Main?.Navigate(new RequireLoginPage());
                    return;
                }
            }

            var company = _context.RenterCompanies
                .FirstOrDefault(c => c.CreatedBy == _currentUser.UserId);

            if (company == null)
            {
                // Belum punya perusahaan
                SectionWelcome.Visibility = Visibility.Visible;
                SectionWaiting.Visibility = Visibility.Collapsed;
                SectionVerified.Visibility = Visibility.Collapsed;
            }
            else if (company.Status == CompanyStatus.Validating)
            {
                // Sudah daftar tapi menunggu verifikasi admin
                SectionWelcome.Visibility = Visibility.Collapsed;
                SectionWaiting.Visibility = Visibility.Visible;
                SectionVerified.Visibility = Visibility.Collapsed;
            }
            else if (company.Status == CompanyStatus.Active)
            {
                SectionWelcome.Visibility = Visibility.Collapsed;
                SectionWaiting.Visibility = Visibility.Collapsed;
                SectionVerified.Visibility = Visibility.Visible;
            }

        }


        // === BUTTON FOR VERIFIED DASHBOARD ===

        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            // Nanti arahkan ke halaman booking
            Main?.Navigate(new BookingPage());
        }

        private void BtnSeeAllNotification_Click(object sender, RoutedEventArgs e)
        {
            // Nanti arahkan ke halaman list notifikasi
            Main?.Navigate(new NotificationPage());
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

        private void BtnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new CompanyFormPage());
        }

        private void BtnJoinExisting_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new JoinCompanyForm());
        }
    }
}
