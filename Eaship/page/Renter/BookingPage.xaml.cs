using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class BookingPage : Page
    {
        private readonly EashipDbContext _context;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public BookingPage()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn || Session.CurrentUser == null)
            {
                MessageBox.Show("You must login first!");
                return;
            }

            var currentUser = Session.CurrentUser;

            string origin = TxtOrigin.Text.Trim();
            string destination = TxtDestination.Text.Trim();
            string cargo = TxtCargo.Text.Trim();

            if (!int.TryParse(TxtDuration.Text, out int duration))
            {
                MessageBox.Show("Duration must be a number!");
                return;
            }

            if (DpStartDate.SelectedDate is not DateTime startDate)
            {
                MessageBox.Show("Please choose start date!");
                return;
            }

            if (string.IsNullOrWhiteSpace(origin) ||
                string.IsNullOrWhiteSpace(destination) ||
                string.IsNullOrWhiteSpace(cargo))
            {
                MessageBox.Show("All fields must be filled!");
                return;
            }

            // Create booking
            var booking = new Booking
            {
                UserId = currentUser.UserId,
                OriginPort = origin,
                DestinationPort = destination,
                StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
                DurationDays = duration,
                CargoDesc = cargo
            };

            booking.SetStatus(BookingStatus.Requested);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            MessageBox.Show("Booking submitted successfully!");

            // Redirect
            Main?.Navigate(new MyBookingPage());
        }




        // ================= NAVBAR =================

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
    }
}
