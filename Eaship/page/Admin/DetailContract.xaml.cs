using Eaship.Models;
using Eaship.Helper;
using Eaship.Models.nondb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class DetailContract : Page
    {
        private readonly EashipDbContext _context;
        private readonly long _contractId;
        private Contract? _contract;

        public DetailContract(long contractId)
        {
            InitializeComponent();
            _contractId = contractId;
            _context = App.Services.GetRequiredService<EashipDbContext>();
            

            LoadContract();
        }

        private async void LoadContract()
        {
            _contract = await _context.Contracts
                .Include(c => c.Booking)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(c => c.ContractId == _contractId);

            MessageBox.Show( $"DEBUG:\nContractId passed = {_contractId}\nPdfUrl in DB = {_contract?.PdfUrl}");

            if (_contract == null)
            {
                MessageBox.Show("Contract not found");
                return;
            }

            // 🟢 CEK APAKAH PDF ADA FILE NYA
            if (!string.IsNullOrEmpty(_contract.PdfUrl) && File.Exists(_contract.PdfUrl))
            {
                PdfViewer.Navigate(new Uri(_contract.PdfUrl, UriKind.Absolute));
            }
            else
            {
                PdfViewer.NavigateToString("<html><body><h2>No PDF Available</h2></body></html>");
            }
        }




        /* NAVIGATION */
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
