using Eaship.Helper;
using Eaship.Models;
using Eaship.Models.nondb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class ContractPayment : Page
    {
        private readonly EashipDbContext _context;

        public ContractPayment()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();

            LoadContracts();
        }

        // ======================
        //  LOAD CONTRACT LIST
        // ======================
        private async void LoadContracts()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Booking)
                .ToListAsync();

            ContractList.ItemsSource = contracts
                .Select(ContractMapper.ToPreview)
                .ToList();
        }

        // ======================
        // NAVIGATE TO DETAIL
        // ======================
        private void SeeMore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ContractPreviewDTO dto)
            {
                Navigate(new DetailContract(dto.ContractId));
            }
        }

        // ======================
        //  UNIVERSAL NAVIGATE()
        // ======================
        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        // ======================
        // SIDEBAR HANDLERS
        // ======================

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
