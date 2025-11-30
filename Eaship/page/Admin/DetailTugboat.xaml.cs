using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eaship.page.Admin
{
    public partial class DetailTugboat : Page
    {
        private readonly EashipDbContext _context;
        private Tugboat? _tugboat;
        private readonly long _id;

        public DetailTugboat(long id)
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            _id = id;

            Loaded += DetailTugboat_Loaded;
        }

        private async void DetailTugboat_Loaded(object sender, RoutedEventArgs e)
        {
            _tugboat = await _context.Tugboats.FirstOrDefaultAsync(t => t.TugboatId == _id);

            if (_tugboat == null)
            {
                MessageBox.Show("Tugboat tidak ditemukan.");
                Navigate(new TugboatListAdmin());
                return;
            }

            // Fill UI
            NameText.Text = _tugboat.Nama;
            HpText.Text = _tugboat.TugboatHp;
            StatusText.Text = _tugboat.Status.ToString();

            if (!string.IsNullOrEmpty(_tugboat.PhotoUrl))
            {
                try
                {
                    TugboatImage.Source = new BitmapImage(new Uri(_tugboat.PhotoUrl));
                }
                catch
                {
                    // fallback
                    TugboatImage.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Eaship;component/Assets/tugboat_default.jpeg"));
                }
            }
            else
            {
                // fallback
                TugboatImage.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Eaship;component/Assets/tugboat_default.jpeg"));
            }


            // =========================
            // CEK APAKAH TUGBOAT SEDANG DIPAKAI DI KONTRAK
            // =========================
            var activeContract = await _context.Contracts
                .Include(c => c.Tongkang)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.RenterCompany)
                .FirstOrDefaultAsync(c =>
                    c.TugboatId == _id &&
                    c.Status == ContractStatus.Approved);

            if (activeContract != null)
            {
                var tongkangName = activeContract.Tongkang?.Name ?? "Unknown";
                var renter = activeContract.Booking?.User?.RenterCompany?.Nama ?? "-";
                var durasi = activeContract.Booking?.DurationDays ?? 0;

                TongkangInfo.Text =
                    $"Sedang Digunakan\n\n" +
                    $"Booking ID: {activeContract.BookingId}\n" +
                    $"Tongkang: {tongkangName}\n" +
                    $"Renter: {renter}\n" +
                    $"Durasi: {durasi} hari\n" +
                    $"Status Kontrak: {activeContract.Status}";
            }
            else
            {
                TongkangInfo.Text = "Tugboat tidak sedang digunakan.";
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new EditTugboat(_id));
        }

        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

    }
}
