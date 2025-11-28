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

        private void LoadLatestNotification()
        {
            var notif = _context.Notifications
                .Where(n => n.UserId == _currentUser.UserId)
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefault();

            if (notif != null)
            {
                TxtNotifTitle.Text = notif.Title;
                TxtNotifMessage.Text = notif.Message;
                TxtNotifTime.Text = notif.CreatedAt.ToString("dd MMM yyyy HH:mm");
            }
            else
            {
                TxtNotifTitle.Text = "No Notifications Yet";
                TxtNotifMessage.Text = "You currently have no notifications.";
                TxtNotifTime.Text = "";
            }
        }



        private void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            // Pastikan user login
            _currentUser ??= Session.CurrentUser;
            if (_currentUser == null)
            {
                MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            // Ambil perusahaan user
            var company = _context.RenterCompanies
                .FirstOrDefault(c => c.RenterCompanyId == _currentUser.RenterCompanyId);

            // CASE 1 — Belum punya perusahaan
            if (company == null)
            {
                ShowSection(welcome: SectionWelcome);
                return;
            }

            // CASE 2 — Menunggu verifikasi admin
            if (company.Status == CompanyStatus.Validating)
            {
                ShowSection(waiting: SectionWaiting);
                return;
            }

            // CASE 3 — Aktif (verified)
            if (company.Status == CompanyStatus.Active)
            {
                ShowSection(verified: SectionVerified);

                LoadLatestNotification();
                return;
            }

            // CASE 4 — Ditolak
            if (company.Status == CompanyStatus.Rejected)
            {
                TxtRejectedReason.Text = company.RejectedReason;
                ShowSection(rejected: SectionRejected);
                return;
            }
        }


        private void ShowSection(Border? welcome = null, Border? waiting = null, Grid? verified = null, Border? rejected = null)
        {
            SectionWelcome.Visibility = welcome != null ? Visibility.Visible : Visibility.Collapsed;
            SectionWaiting.Visibility = waiting != null ? Visibility.Visible : Visibility.Collapsed;
            SectionVerified.Visibility = verified != null ? Visibility.Visible : Visibility.Collapsed;
            SectionRejected.Visibility = rejected != null ? Visibility.Visible : Visibility.Collapsed;
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
