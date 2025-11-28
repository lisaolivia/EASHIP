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
                .Include(b => b.User)
                    .ThenInclude(u => u.RenterCompany)
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

            TongkangBox.ItemsSource = await _context.Tongkangs.ToListAsync();
            TongkangBox.DisplayMemberPath = "Name";
            TongkangBox.SelectedValuePath = "TongkangId";

            TugboatBox.ItemsSource = await _context.Tugboats.ToListAsync();
            TugboatBox.DisplayMemberPath = "Nama";
            TugboatBox.SelectedValuePath = "TugboatId";
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null)
            {
                MessageBox.Show("Booking not loaded.");
                return;
            }

            // VALIDASI TONGKANG
            if (TongkangBox.SelectedItem is not Tongkang selectedTongkang)
            {
                MessageBox.Show("Select a tongkang!");
                return;
            }

            // VALIDASI DURASI
            if (!int.TryParse(DaysBox.Text, out int days))
            {
                MessageBox.Show("Invalid days!");
                return;
            }

            // ======================================================
            // 1. TAMBAHKAN RELASI BOOKING–TONGKANG TANPA DUPLIKASI
            // ======================================================
            var existingRelation = await _context.BookingTongkangs
                .FirstOrDefaultAsync(bt =>
                    bt.BookingId == _booking.BookingId &&
                    bt.TongkangId == selectedTongkang.TongkangId);

            if (existingRelation == null)
            {
                var relation = new BookingTongkang
                {
                    BookingId = _booking.BookingId,
                    TongkangId = selectedTongkang.TongkangId
                };

                _context.BookingTongkangs.Add(relation);
            }

            // ======================================================
            // 2. UPDATE STATUS BOOKING + UPDATE TONGKANG
            // ======================================================
            _booking.SetStatus(BookingStatus.Confirmed);
            selectedTongkang.MarkUnavailable();

            await _context.SaveChangesAsync();

            // ======================================================
            // 3. RELOAD BOOKING SECARA LENGKAP (User + RenterCompany)
            // ======================================================
            var bookingFull = await _context.Bookings
                .Include(b => b.User)
                    .ThenInclude(u => u.RenterCompany)
                .Include(b => b.BookingTongkangs)
                    .ThenInclude(bt => bt.Tongkang)
                .FirstOrDefaultAsync(b => b.BookingId == _booking.BookingId);

            if (bookingFull == null)
            {
                MessageBox.Show("Failed to reload booking.");
                return;
            }

            if (bookingFull.User == null)
            {
                MessageBox.Show("Booking has no user data!");
                return;
            }

            if (bookingFull.User.RenterCompany == null)
            {
                MessageBox.Show("User has no registered company!");
                return;
            }

            var renterCompany = bookingFull.User.RenterCompany;

            // ======================================================
            // 4. CREATE CONTRACT RECORD
            // ======================================================
            var contract = new Contract
            {
                BookingId = bookingFull.BookingId
            };

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();   // agar ContractId terisi

            // ======================================================
            // 5. GENERATE CONTRACT PDF
            // ======================================================
            string pdfPath = ContractPdfGenerator.GenerateContractPdf(
                contract,
                bookingFull,
                renterCompany
            );

            contract.PdfUrl = pdfPath;

            // ======================================================
            // 6. SIMPAN URL PDF KE DATABASE
            // ======================================================
            contract.PdfUrl = pdfPath;
            contract.MarkPending();
            await _context.SaveChangesAsync();

            MessageBox.Show("Booking confirmed & contract generated!");
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
