using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class Barges : Page
    {
        private readonly EashipDbContext _context;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public Barges()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            Loaded += Barges_Loaded;
        }

        private async void Barges_Loaded(object sender, RoutedEventArgs e)
        {
            // load default data
            await LoadTongkang();
            await LoadTugboat();
        }


        // ===================== LOAD TONGKANG =====================
        private async Task LoadTongkang(string? origin = null, string? dest = null,
                               string? loadType = null, int? minDwt = null)
        {
            // 1. ambil dulu dari database (tanpa parsing)
            var list = await _context.Tongkangs
                            .Where(t => t.Status == TongkangStatus.Available)
                            .ToListAsync();

            // 2. lakukan filter DWT di memory (C# murni)
            if (minDwt.HasValue)
            {
                list = list.Where(t =>
                {
                    if (int.TryParse(
                            t.KapasitasDwt.Replace("DWT", "").Trim(),
                            out int dwt))
                    {
                        return dwt >= minDwt.Value;
                    }

                    return false;
                }).ToList();
            }

            TongkangList.ItemsSource = list;
        }



        // ===================== LOAD TUGBOAT =====================
        private async Task LoadTugboat()
        {
            var tugboats = await _context.Tugboats
                                .Where(x => x.Status == TugboatStatus.AVAILABLE)
                                .ToListAsync();

            TugboatList.ItemsSource = tugboats;
        }


        // ===================== SEARCH BUTTON =====================
        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string origin = TxtOrigin.Text.Trim();
            string dest = TxtDestination.Text.Trim();
            string loadType = (ComboLoad.SelectedItem as ComboBoxItem)?.Content?.ToString();
            int? minDwt = null;

            if (int.TryParse(TxtCapacity.Text.Trim(), out int d))
                minDwt = d;

            await LoadTongkang(origin, dest, loadType, minDwt);
        }


        // ===================== TONGKANG DETAILS =====================
        private void BtnTongkangDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tongkang tongkang)
            {
                // TODO: navigate to detail page
                MessageBox.Show($"Detail Tongkang: {tongkang.Name}");
            }
        }


        // ===================== TUGBOAT DETAILS =====================
        private void BtnTugboatDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tugboat tug)
            {
                // TODO: navigate to detail page
                MessageBox.Show($"Detail Tugboat: {tug.Nama}");
            }
        }
        // ==========================================================
        //                     NAVBAR HANDLERS
        // ==========================================================
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
            Main?.Navigate(new ContractPage()); // ganti kalau nama lain
        }

        private void BtnNotif_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new NotificationPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ProfilPage()); // ganti sesuai nama kamu
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }

    }
}
