using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class ContractPage : Page
    {
        private readonly EashipDbContext _context;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public ContractPage()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            Loaded += ContractPage_Loaded;
        }

        private async void ContractPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn || Session.CurrentUser == null)
            {
                MessageBox.Show("You must login first.");
                return;
            }

            var userId = Session.CurrentUser.UserId;

            var contracts = await _context.Contracts
                .Include(c => c.Booking)
                .Where(c => c.Booking.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var displayList = contracts.Select(c => new ContractDisplayDTO
            {
                ContractId = c.ContractId,
                Origin = c.Booking?.OriginPort ?? "-",
                Destination = c.Booking?.DestinationPort ?? "-",
                Cargo = c.Booking?.CargoDesc ?? "-",
                Status = c.Status.ToString(),
                PdfPath = c.PdfUrl ?? ""
            }).ToList();

            ContractList.ItemsSource = displayList;
        }

        private void BtnOpenPdf(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string pdfPath)
            {
                if (File.Exists(pdfPath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(pdfPath)
                        {
                            UseShellExecute = true
                        });
                    }
                    catch
                    {
                        MessageBox.Show("Failed to open PDF. Check file permissions.");
                    }
                }
                else
                {
                    MessageBox.Show("Contract PDF file not found.");
                }
            }
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long contractId)
            {
                Main?.Navigate(new RenterDetailContract(contractId));
            }
        }

    }

    // DTO khusus tampilan renter
    public class ContractDisplayDTO
    {
        public long ContractId { get; set; }
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
        public string Cargo { get; set; } = "";
        public string Status { get; set; } = "";
        public string PdfPath { get; set; } = "";
    }
}
