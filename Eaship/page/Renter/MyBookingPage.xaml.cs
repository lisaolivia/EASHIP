using Eaship.Models;
using Eaship.page.Admin;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class MyBookingPage : Page
    {
        private readonly EashipDbContext _context;
        private User? _currentUser;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public MyBookingPage()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _currentUser = Session.CurrentUser;

            if (_currentUser == null)
            {
                MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                Main?.Navigate(new LoginPage());
                return;
            }

            LoadBookings();
        }

        // ===========================
        // LOAD BOOKING LIST
        // ===========================
        private void LoadBookings()
        {
            try
            {
                var bookings = _context.Bookings
                    .Include(b => b.BookingTongkangs)
                    .Where(b => b.UserId == _currentUser!.UserId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToList();

                var bookingVm = bookings.Select(b => new
                {
                    BookingId = b.BookingId,
                    BookingCode = $"BK-{b.BookingId:000000}",
                    Origin = $"Origin : {b.OriginPort}",
                    Destination = $"Destination : {b.DestinationPort}",
                    Route = $"{b.OriginPort} → {b.DestinationPort}",
                    DueDate = b.StartDate.AddDays(b.DurationDays).ToString("dd MMM yyyy"),
                    Status = b.Status.ToString(),
                    FreightCost = $"USD {b.HargaTotal:N0}",
                    Outstanding = "USD 0"   // placeholder, nanti dari Invoice
                }).ToList();

                BookingList.ItemsSource = bookingVm;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat booking: " + ex.Message);
            }
        }


        // ===========================
        // BUTTON NAVIGATION
        // ===========================



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
            Main?.Navigate(new ContractPage());
        }

        private void BtnNotif_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new NotificationPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ProfilPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }



        // ===========================
        // BUTTON ACTIONS BOOKING LIST
        // ===========================

        private void BtnViewContract (object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long bookingId)
            {
                Main?.Navigate(new RenterDetailContract(bookingId));
            }
        }

        private void BtnDownloadInvoice(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long bookingId)
            {
                MessageBox.Show($"Download invoice untuk Booking ID: {bookingId}");
                // TODO: implement setelah ada PDF generator
            }
        }

        private void BtnSendEmail(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long bookingId)
            {
                MessageBox.Show($"Invoice untuk Booking ID {bookingId} terkirim ke email!");
                // TODO: connect email service
            }
        }
    }
}
