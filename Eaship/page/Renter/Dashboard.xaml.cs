using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class Dashboard : Page
    {
        private readonly IUserService _users;
        private readonly EashipDbContext _context;
        private User? _currentUser;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public Dashboard()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            Loaded += Dashboard_Loaded;
            _currentUser = Session.CurrentUser;
        }

        private void LoadLatestNotification()
        {
            var notif = _context.Notifications
                .Where(n => n.UserId == _currentUser.UserId)
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefault();

            if (notif != null)
            {
                TxtNotifTitle.Text = notif.Title;
                TxtNotifMessage.Text = notif.Message;
                TxtNotifTime.Text = notif.CreatedAt.ToString("dd MMM yyyy HH:mm");
            }
            else
            {
                TxtNotifTitle.Text = "No Notifications Yet";
                TxtNotifMessage.Text = "You currently have no notifications.";
                TxtNotifTime.Text = "";
            }
        }

        private async void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            _currentUser ??= Session.CurrentUser;
            if (_currentUser == null)
            {
                MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            var company = _context.RenterCompanies
                .FirstOrDefault(c => c.RenterCompanyId == _currentUser.RenterCompanyId);

            // CASE 1 — Belum punya perusahaan
            if (company == null)
            {
                ShowSection(welcome: SectionWelcome);
                return;
            }

            // CASE 2 — Menunggu verifikasi admin
            if (company.Status == CompanyStatus.Validating)
            {
                ShowSection(waiting: SectionWaiting);
                return;
            }

            // CASE 3 — VERIFIED
            if (company.Status == CompanyStatus.Active)
            {
                ShowSection(verified: SectionVerified);

                LoadLatestNotification();

                // 🔥 NEW — LOAD API SECTION
                await LoadPortWeathers();
                await LoadExchangeRate();
                LoadLocalTime();

                return;
            }

            // CASE 4 — Rejected
            if (company.Status == CompanyStatus.Rejected)
            {
                TxtRejectedReason.Text = company.RejectedReason;
                ShowSection(rejected: SectionRejected);
                return;
            }
        }

     private void ShowSection(
     Border? welcome = null,
     Border? waiting = null,
     StackPanel? verified = null,
     Border? rejected = null)

        {
            SectionWelcome.Visibility = welcome != null ? Visibility.Visible : Visibility.Collapsed;
            SectionWaiting.Visibility = waiting != null ? Visibility.Visible : Visibility.Collapsed;
            SectionVerified.Visibility = verified != null ? Visibility.Visible : Visibility.Collapsed;
            SectionRejected.Visibility = rejected != null ? Visibility.Visible : Visibility.Collapsed;
        }



        // =====================================================================
        // 🔥 NEW API 1: WEATHER
        // =====================================================================
        private async Task LoadPortWeathers()
        {
            var ports = new List<(double lat, double lon, TextBlock temp, TextBlock wind)>
    {
        (-6.11, 106.88, TxtP1Temp, TxtP1Wind),  // Tanjung Priok
        (-7.20, 112.73, TxtP2Temp, TxtP2Wind),  // Tanjung Perak
        (-1.27, 116.82, TxtP3Temp, TxtP3Wind),  // Balikpapan
        (-5.14, 119.41, TxtP4Temp, TxtP4Wind),  // Makassar
        (3.79, 98.68, TxtP5Temp, TxtP5Wind)     // Belawan
    };

            using var client = new HttpClient();

            foreach (var port in ports)
            {
                try
                {
                    var url =
                        $"https://api.open-meteo.com/v1/forecast?latitude={port.lat}&longitude={port.lon}&current_weather=true";

                    var json = await client.GetStringAsync(url);
                    using var doc = JsonDocument.Parse(json);

                    var temp = doc.RootElement.GetProperty("current_weather").GetProperty("temperature").GetDouble();
                    var wind = doc.RootElement.GetProperty("current_weather").GetProperty("windspeed").GetDouble();

                    port.temp.Text = $"{temp}°C";
                    port.wind.Text = $"Wind: {wind} km/h";
                }
                catch
                {
                    port.temp.Text = "N/A";
                    port.wind.Text = "";
                }
            }
        }



        // =====================================================================
        // 🔥 NEW API 2: EXCHANGE RATE
        // =====================================================================
        private async Task LoadExchangeRate()
        {
            try
            {
                using var client = new HttpClient();

                var json = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");
                using var doc = JsonDocument.Parse(json);

                var idr = doc.RootElement.GetProperty("rates").GetProperty("IDR").GetDouble();

                TxtRateIDR.Text = $"{idr:N0}";
                TxtRateLastUpdate.Text = "Updated just now";
            }
            catch
            {
                TxtRateIDR.Text = "N/A";
                TxtRateLastUpdate.Text = "";
            }
        }


        // =====================================================================
        // 🔥 NEW API 3: LOCAL TIME (NO REQUEST — LOCAL SYSTEM)
        // =====================================================================
        private void LoadLocalTime()
        {
            var now = DateTime.Now;
            TxtTime.Text = now.ToString("HH:mm");
            TxtDate.Text = now.ToString("dddd, dd MMM yyyy");
        }


        // =====================================================================
        // BUTTONS
        // =====================================================================

        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new BookingPage());
        }

        private void BtnSeeAllNotification_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new NotificationPage());
        }

        private void BtnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new CompanyFormPage());
        }

        private void BtnJoinExisting_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new JoinCompanyForm());
        }
    }
}
