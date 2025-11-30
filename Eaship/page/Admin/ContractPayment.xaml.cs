using Eaship.Helper;
using Eaship.Models;
using Eaship.Models.nondb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    // ============================
    // VIEWMODEL UNTUK DISPLAY LIST
    // ============================
    public class ContractPaymentItem
    {
        public long ContractId { get; set; }
        public string CompanyName { get; set; } = "Unknown";
        public string CompanyAddress { get; set; } = "-";
        public DateTime CreatedAt { get; set; }
        public string? PdfUrl { get; set; }
    }

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
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.RenterCompany)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var list = contracts.Select(c => new ContractPreviewDTO
            {
                ContractId = c.ContractId,
                CompanyName = c.Booking.User.RenterCompany?.Nama ?? "-",
                CompanyAddress = c.Booking.User.RenterCompany?.Address ?? "-",
                CreatedAt = c.CreatedAt,
                PdfUrl = c.PdfUrl,
                Status = c.Status.ToString() 
            }).ToList();


            ContractList.ItemsSource = list;
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
        // UNIVERSAL NAVIGATE()
        // ======================
        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        // ======================
        // SIDEBAR HANDLERS
        // ======================

  
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }

    }
}
