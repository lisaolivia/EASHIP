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
        private async Task LoadTongkang(string? name = null, int? minDwt = null)
        {
            var list = await _context.Tongkangs
                            .Where(t => t.Status == TongkangStatus.Available)
                            .ToListAsync();

            if (!string.IsNullOrWhiteSpace(name))
                list = list.Where(t =>
                    t.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minDwt.HasValue)
            {
                list = list.Where(t =>
                {
                    if (int.TryParse(
                            t.KapasitasDwt.Replace("DWT", "").Trim(),
                            out int dwt))
                        return dwt >= minDwt.Value;

                    return false;
                }).ToList();
            }

            TongkangList.ItemsSource = list;
        }

        // ===================== LOAD TUGBOAT =====================
        private async Task LoadTugboat(string? name = null, int? minHp = null)
        {
            var tugs = await _context.Tugboats
                            .Where(t => t.Status == TugboatStatus.AVAILABLE)
                            .ToListAsync();

            if (!string.IsNullOrWhiteSpace(name))
                tugs = tugs.Where(t =>
                    t.Nama.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minHp.HasValue)
            {
                tugs = tugs.Where(t =>
                {
                    if (int.TryParse(
                            t.TugboatHp.Replace("HP", "").Trim(),
                            out int realHp))
                        return realHp >= minHp.Value;

                    return false;
                }).ToList();
            }

            TugboatList.ItemsSource = tugs;
        }




        // ===================== SEARCH BUTTON =====================
        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchName = TxtSearchName.Text.Trim();

            int? minDwt = null;
            if (int.TryParse(TxtMinDwt.Text.Trim(), out int d))
                minDwt = d;

            int? minHp = null;
            if (int.TryParse(TxtMinHp.Text.Trim(), out int hp))
                minHp = hp;

            // Tongkang filter by: Name + DWT
            await LoadTongkang(searchName, minDwt);

            // Tugboat filter by: Name + HP
            await LoadTugboat(searchName, minHp);
        }






        // ===================== TONGKANG DETAILS =====================
        private void BtnTongkangDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long tongkangId)
            {
                Main?.Navigate(new DetailTongkang(tongkangId));
            }
        }



        // ===================== TUGBOAT DETAILS =====================
        private void BtnTugboatDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is long tugboatId)
            {
                var tug = _context.Tugboats.FirstOrDefault(x => x.TugboatId == tugboatId);
                if (tug != null)
                {
                    Main?.Navigate(new DetailTugboat(tug));
                }
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
