using Eaship.Models;
using Eaship.Services;
using Eaship.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class Notifikasi : Page
    {
        private readonly ICompanyService _companies;
        private readonly IBookingService _bookings;
        private readonly IContractService _contracts;

        public Notifikasi()
        {
            InitializeComponent();

            _companies = App.Services.GetRequiredService<ICompanyService>();
            _bookings = App.Services.GetRequiredService<IBookingService>();
            _contracts = App.Services.GetRequiredService<IContractService>();

            Loaded += Notifikasi_Loaded;
        }

        private async void Notifikasi_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<object>();

            // 1. Company Pending
            var pendingCompanies = await _companies.GetPendingAsync();
            list.AddRange(
                pendingCompanies.Select(c => new
                {
                    Title = "Company Verification Pending",
                    Description = $"{c.Nama} | NPWP: {c.NPWP}",
                    Date = c.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                })
            );

            // 2. Booking Requests
            var pendingBookings = await _bookings.GetPendingAsync();
            list.AddRange(
                pendingBookings.Select(b => new
                {
                    Title = "Booking Request",
                    Description = $"Booking #{b.BookingId} | {b.OriginPort} → {b.DestinationPort}",
                    Date = b.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                })
            );

            // 3. Contract Pending
            var pendingContracts = await _contracts.GetPendingAsync();
            list.AddRange(
                pendingContracts.Select(c => new
                {
                    Title = "Contract Pending",
                    Description = $"Contract #{c.ContractId} | {c.Booking?.RenterCompany?.Nama ?? "Unknown Company"}",
                    Date = c.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                })
            );

            NotificationList.ItemsSource = list;
        }

        // ===== NAVIGATION =====
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        private void Navigate(Page page) => Main?.Navigate(page);

     

    }
}
