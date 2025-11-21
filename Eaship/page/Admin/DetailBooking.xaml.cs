using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class DetailBooking : Page
    {
        private readonly EashipDbContext _context;
        private readonly long _bookingId;
        private Booking? _booking;

        public DetailBooking(long bookingId)
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _bookingId = bookingId;

            LoadData();
        }

        private async void LoadData()
        {
            _booking = await _context.Bookings
                .Include(b => b.BookingTongkangs)
                .ThenInclude(bt => bt.Tongkang)
                .FirstOrDefaultAsync(b => b.BookingId == _bookingId);

            if (_booking == null)
            {
                MessageBox.Show("Booking not found");
                return;
            }

            OriginBox.Text = _booking.OriginPort;
            DestinationBox.Text = _booking.DestinationPort;
            StartBox.Text = _booking.StartDate.ToString("dd MMM yyyy");
            DurationBox.Text = _booking.DurationDays.ToString();
            CargoBox.Text = _booking.CargoDesc;
            StatusBox.Text = _booking.Status.ToString();

            var tongkangs = await _context.Tongkangs.ToListAsync();
            TongkangBox.ItemsSource = tongkangs;
            TongkangBox.DisplayMemberPath = "Name";
            TongkangBox.SelectedValuePath = "TongkangId";

            var tugboats = await _context.Tugboats.ToListAsync();
            TugboatBox.ItemsSource = tugboats;
            TugboatBox.DisplayMemberPath = "Nama";
            TugboatBox.SelectedValuePath = "TugboatId";
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null)
                return;

            if (TongkangBox.SelectedItem is not Tongkang selected)
            {
                MessageBox.Show("Select tongkang!");
                return;
            }

            if (!int.TryParse(DaysBox.Text, out int days))
            {
                MessageBox.Show("Invalid days!");
                return;
            }

            var relation = new BookingTongkang
            {
                BookingId = _booking.BookingId,
                TongkangId = selected.TongkangId
            };

            _context.BookingTongkangs.Add(relation);

            _booking.SetStatus(BookingStatus.Confirmed);
            selected.MarkUnavailable();

            await _context.SaveChangesAsync();

            MessageBox.Show("Confirmed!");
        }

        private async void Decline_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null)
                return;

            _booking.SetStatus(BookingStatus.Cancelled);
            await _context.SaveChangesAsync();

            MessageBox.Show("Declined");
        }

        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }

    }
}
