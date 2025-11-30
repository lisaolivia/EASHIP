using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class DashboardAdmin : Page
    {
        private readonly EashipDbContext _context;
        private readonly ICompanyService _companies;

        public DashboardAdmin()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _companies = App.Services.GetRequiredService<ICompanyService>();

            Loaded += DashboardAdmin_Loaded;
        }

        private async void DashboardAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. COMPANY
            var pendingCompany = await _companies.GetPendingAsync();
            var activeCompany = await _companies.GetActiveAsync();
            txtCompanyVerified.Text = activeCompany.Count.ToString();

            // 2. TONGKANG (status: Available)
            int tongkangAktif = await _context.Tongkangs
                .Where(t => t.Status == TongkangStatus.Available)
                .CountAsync();
            txtTongkangAktif.Text = tongkangAktif.ToString();

            // 3. TUGBOAT (status: AVAILABLE)
            int tugboatAktif = await _context.Tugboats
                .Where(t => t.Status == TugboatStatus.AVAILABLE)
                .CountAsync();
            txtTugboatAktif.Text = tugboatAktif.ToString();

            // 4. BOOKING (status: Requested = pending)
            int bookingPending = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Requested)
                .CountAsync();
            txtBookingPending.Text = bookingPending.ToString();

            // 5. NOTIFICATION
            NotificationList.ItemsSource = pendingCompany.Select(c => new
            {
                Title = "Company Verification",
                Company = c.Nama,
                NPWP = c.NPWP,
                Date = c.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
            });
        }



        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }
    }
}
