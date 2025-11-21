using Eaship.Models;
using Eaship.Models.nondb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Eaship.Helper;

namespace Eaship.page.Admin
{
    public partial class BookingRequest : Page
    {
        private readonly EashipDbContext _context;

        public BookingRequest()
        {
            InitializeComponent();

            // Ambil DB context
            _context = App.Services.GetRequiredService<EashipDbContext>();

            // Load data booking saat page dibuka
            LoadBookingList();
        }

        private async void LoadBookingList()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.BookingTongkangs)
                    .ThenInclude(bt => bt.Tongkang)
                    .ToListAsync();

                // Convert ke DTO preview
                var previewList = bookings
                    .Select(b => BookingMapper.ToPreview(b))
                    .ToList();

                // Bind ke ItemsControl
                BookingList.ItemsSource = previewList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal load data booking: {ex.Message}");
            }
        }

        private void SeeMore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is BookingPreviewDTO dto)
            {
                var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
                frame?.Navigate(new DetailBooking(dto.BookingId));
            }
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
