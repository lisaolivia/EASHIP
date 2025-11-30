using Eaship.Helper;
using Eaship.Models;
using Eaship.Models.nondb;
using Eaship.page.Renter;
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
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;


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
                .Include(c => c.Tongkang)
                .Include(c => c.Tugboat)
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

        private async void CompleteContract_Click(object sender, RoutedEventArgs e)
        {
            if (_contract == null)
            {
                MessageBox.Show("Contract tidak ditemukan.");
                return;
            }

            // Reload lengkap dari DB (dengan Tongkang & Tugboat)
            var contract = await _context.Contracts
                .Include(c => c.Tongkang)
                .Include(c => c.Tugboat)
                .FirstAsync(c => c.ContractId == _contract.ContractId);

            // 1. Update status contract
            contract.Complete();

            // 2. Bebaskan tongkang
            if (contract.Tongkang != null)
            {
                contract.Tongkang.SetStatus(TongkangStatus.Available);
            }

            // 3. Bebaskan tugboat
            if (contract.Tugboat != null)
            {
                contract.Tugboat.SetStatus(TugboatStatus.AVAILABLE);
            }

            await _context.SaveChangesAsync();

            MessageBox.Show("Kontrak selesai! Semua kapal tersedia kembali.");

            Navigate(new ContractPayment());
        }



        /* NAVIGATION */
        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

       
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }
    }
}
