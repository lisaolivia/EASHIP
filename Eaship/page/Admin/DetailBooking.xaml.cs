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


            TongkangBox.ItemsSource = await _context.Tongkangs
            .Where(t => t.Status == TongkangStatus.Available)
            .ToListAsync();

            TongkangBox.DisplayMemberPath = "Name";
            TongkangBox.SelectedValuePath = "TongkangId";

            TugboatBox.ItemsSource = await _context.Tugboats.ToListAsync();
            TugboatBox.DisplayMemberPath = "Nama";
            TugboatBox.SelectedValuePath = "TugboatId";

            // Disable tombol kalau booking sudah bukan Requested
            if (_booking.Status != BookingStatus.Requested)
            {
                ConfirmButton.IsEnabled = false;
                DeclineButton.IsEnabled = false;
            }
        }


        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null)
            {
                MessageBox.Show("Booking not loaded.");
                return;
            }

            if (_booking.Status != BookingStatus.Requested)
            {
                MessageBox.Show("Only requested bookings can be approved.");
                return;
            }

            // VALIDASI TONGKANG
            if (TongkangBox.SelectedItem is not Tongkang selectedTongkang)
            {
                MessageBox.Show("Select a tongkang!");
                return;
            }


            // ============== 1. Tambahkan relasi booking - tongkang ============
            var existingRelation = await _context.BookingTongkangs
                .FirstOrDefaultAsync(bt =>
                    bt.BookingId == _booking.BookingId &&
                    bt.TongkangId == selectedTongkang.TongkangId);

            if (existingRelation == null)
            {
                _context.BookingTongkangs.Add(new BookingTongkang
                {
                    BookingId = _booking.BookingId,
                    TongkangId = selectedTongkang.TongkangId
                });
            }

            // ============== 2. Approve booking & assign tongkang ===============

            // CEK STATUS TONGKANG DULU

            bool isUsedInContract = await _context.Contracts
            .AnyAsync(c => c.TongkangId == selectedTongkang.TongkangId
                        && c.Status == ContractStatus.Approved);

            if (isUsedInContract)
            {
                MessageBox.Show("Tongkang sedang dipakai oleh kontrak lain!", "Error");
                return;
            }


            if (selectedTongkang.Status != TongkangStatus.Available)
            {
                MessageBox.Show("Tongkang ini sedang digunakan dan tidak bisa diassign lagi!",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _booking.Approve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (TugboatBox.SelectedItem is Tugboat selectedTugboat)
            {
                bool tugboatUsed = await _context.Contracts
                    .AnyAsync(c => c.TugboatId == selectedTugboat.TugboatId
                                && c.Status == ContractStatus.Approved);

                if (tugboatUsed)
                {
                    MessageBox.Show("Tugboat sedang dipakai kontrak lain!", "Error");
                    return;
                }

                if (selectedTugboat.Status != TugboatStatus.AVAILABLE)
                {
                    MessageBox.Show("Tugboat ini tidak tersedia!", "Error");
                    return;
                }

                selectedTugboat.SetAssigned();
                _context.Tugboats.Update(selectedTugboat);
            }


            // SET STATUS
            selectedTongkang.SetStatus(TongkangStatus.Assigned);
            _context.Tongkangs.Update(selectedTongkang);
            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();

            // ============== 3. Reload booking lengkap ==========================
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

            var renterCompany = bookingFull.User?.RenterCompany;
            if (renterCompany == null)
            {
                MessageBox.Show("User has no company data.");
                return;
            }

            // ============== 4. Create contract ================================
            var contract = new Contract
            {
                BookingId = bookingFull.BookingId,
                TongkangId = selectedTongkang.TongkangId,
                TugboatId = (TugboatBox.SelectedItem as Tugboat)?.TugboatId
            };


            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            // ============== 5. GENERATE PDF ===================================
            string pdfPath = ContractPdfGenerator.GenerateContractPdf(
                contract,
                bookingFull,
                renterCompany
            );

            contract.PdfUrl = pdfPath;
            contract.MarkPending();

            await _context.SaveChangesAsync();

            MessageBox.Show("Booking approved & contract generated!");

            // ============== 6. Create notification ============================
            var notif = App.Services.GetRequiredService<INotificationService>();

            notif.Create(
                bookingFull.UserId,
                "BookingApproved",
                "Booking Approved",
                $"Your booking #{bookingFull.BookingId} has been approved and a contract has been generated.",
                bookingId: bookingFull.BookingId,
                contractId: contract.ContractId
            );

            ConfirmButton.IsEnabled = false;
            DeclineButton.IsEnabled = false;
        }


        private async void Decline_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null)
            {
                MessageBox.Show("Booking not loaded.");
                return;
            }

            if (_booking.Status != BookingStatus.Requested)
            {
                MessageBox.Show("This booking can no longer be declined.");
                return;
            }

            // ============ Decline Booking (VALIDATED) ==============
            try
            {
                _booking.Decline();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            await _context.SaveChangesAsync();

            // ============ Reload User =============
            var bookingFull = await _context.Bookings
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == _booking.BookingId);

            if (bookingFull?.User == null)
            {
                MessageBox.Show("Failed to load user.");
                return;
            }

            // ============ Notification =============
            var notif = App.Services.GetRequiredService<INotificationService>();

            notif.Create(
                bookingFull.User.UserId,
                "BookingDeclined",
                "Booking Declined",
                $"Your booking #{bookingFull.BookingId} has been declined by admin.",
                bookingId: bookingFull.BookingId
            );

            MessageBox.Show("Booking declined!");

            ConfirmButton.IsEnabled = false;
            DeclineButton.IsEnabled = false;
        }
    }
}
