using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class RenterDetailContract : Page
    {
        private readonly long _contractId;
        private readonly EashipDbContext _context;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RenterDetailContract(long contractId)
        {
            InitializeComponent();
            _contractId = contractId;
            _context = App.Services.GetRequiredService<EashipDbContext>();

            LoadContract();
        }

        private async void LoadContract()
        {
            var data = await _context.Contracts
                .Include(c => c.Booking)
                .FirstOrDefaultAsync(c => c.ContractId == _contractId);

            if (data == null)
            {
                MessageBox.Show("Contract not found.");
                return;
            }

            TxtContractId.Text = data.ContractId.ToString();
            TxtOrigin.Text = data.Booking?.OriginPort ?? "-";
            TxtDestination.Text = data.Booking?.DestinationPort ?? "-";
            TxtCargo.Text = data.Booking?.CargoDesc ?? "-";
            TxtStatus.Text = data.Status.ToString();

            if (!string.IsNullOrEmpty(data.PdfUrl) && File.Exists(data.PdfUrl))
                PdfViewer.Navigate(new Uri(data.PdfUrl, UriKind.Absolute));
            else
                PdfViewer.NavigateToString("<html><body><h2>No PDF Available</h2></body></html>");
        }

    }
}
