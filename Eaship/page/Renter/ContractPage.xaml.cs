using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
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

        private void ContractPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContracts();
        }

        private void LoadContracts()
        {
            // Mock Data dulu — nanti kamu sambungkan ke Postgres
            ContractList.ItemsSource = new[]
            {
                new {
                    ContractId = 1,
                    Route = "CNSHA → Los Angeles",
                    Origin = "Port of Shanghai, China",
                    Destination = "Port of Los Angeles, USA",
                    BookingCode = "MBSS #BK-2025-112",
                    DueDate = "11 days",
                    Status = "Paid",
                    FreightCost = "USD 3,000",
                    Outstanding = "USD 0"
                },
                new {
                    ContractId = 2,
                    Route = "CNSHA → Tokyo",
                    Origin = "Port of Shanghai, China",
                    Destination = "Tokyo Port, Japan",
                    BookingCode = "MBSS #BK-2025-120",
                    DueDate = "7 days",
                    Status = "Unpaid",
                    FreightCost = "USD 2,800",
                    Outstanding = "USD 2,800"
                }
            };
        }

        private void BtnDownloadInvoice(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Download invoice triggered!");
        }

        private void BtnSendEmail(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Invoice sent to email!");
        }

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
