using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Eaship.page.Admin
{
    public partial class DetailTongkang : Page
    {
        private readonly EashipDbContext _context;
        private readonly long _id;
        private Tongkang _tongkang;

        public DetailTongkang(long id)
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _id = id;

            LoadData();
        }

        private async void LoadData()
        {
            _tongkang = await _context.Tongkangs.FirstOrDefaultAsync(t => t.TongkangId == _id);

            if (_tongkang == null)
            {
                NameText.Text = "Tongkang tidak ditemukan";
                return;
            }

            NameText.Text = _tongkang.Name;
            KapasitasText.Text = _tongkang.KapasitasDwt + " DWT";
            StatusText.Text = _tongkang.Status.ToString();
            IncludeTugboatText.Text = _tongkang.IncludeTugboat ? "Ya" : "Tidak";
            JumlahTugboatText.Text = _tongkang.TugboatIds?.Count.ToString() ?? "0";


            // IMAGE SEMENTARA STATIC
            // ===== FOTO FROM CLOUDINARY =====
            if (!string.IsNullOrWhiteSpace(_tongkang.PhotoUrl))
            {
                try
                {
                    TongkangImage.Source = new BitmapImage(new Uri(_tongkang.PhotoUrl, UriKind.Absolute));
                }
                catch
                {
                    TongkangImage.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Eaship;component/Assets/default_barge.jpg")
                    );
                }
            }
            else
            {
                TongkangImage.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Eaship;component/Assets/default_barge.jpg")
                );
            }

            // =========================
            // STATUS PENGGUNAAN TONGKANG
            // =========================
            var activeContract = await _context.Contracts
                .Include(c => c.Booking)
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.RenterCompany)
                .Include(c => c.Tugboat)
                .FirstOrDefaultAsync(c =>
                    c.TongkangId == _id &&
                    c.Status == ContractStatus.Approved);

            if (activeContract != null)
            {
                var renter = activeContract.Booking?.User?.RenterCompany?.Nama ?? "Perusahaan tidak ditemukan";
                var tugboatName = activeContract.Tugboat?.Nama ?? "Tidak ada tugboat";
                var durasi = activeContract.Booking?.DurationDays ?? 0;

                CompanyText.Text =
                    $"Sedang Digunakan\n\n" +
                    $"Booking ID: {activeContract.BookingId}\n" +
                    $"Renter: {renter}\n" +
                    $"Tugboat: {tugboatName}\n" +
                    $"Durasi: {durasi} hari\n" +
                    $"Status Kontrak: {activeContract.Status}";
            }
            else
            {
                CompanyText.Text = "Tongkang tidak sedang digunakan.";
            }

            // COMPANY (jika kamu aktifkan lagi relasi)
            CompanyText.Text = "Belum ada";
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new EditTongkang(_id));
        }

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
