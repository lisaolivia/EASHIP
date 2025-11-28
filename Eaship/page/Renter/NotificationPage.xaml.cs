using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using Eaship.Models;
using System.Windows.Controls;


namespace Eaship.page.Renter
{
    public partial class NotificationPage : Page
    {
        private readonly INotificationService _notifs;
        private readonly User? _user;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public NotificationPage()
        {
            InitializeComponent();
            _notifs = App.Services.GetRequiredService<INotificationService>();
            _user = Session.CurrentUser;
            Loaded += NotificationPage_Loaded;
        }

        private void NotificationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_user == null)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            LoadNotifications();
        }

        private void LoadNotifications()
        {
            var list = _notifs.GetAll(_user.UserId);

            if (!list.Any())
            {
                NotifList.ItemsSource = new[]
                {
                    new { Title="No Notifications Yet", Description="You currently have no notifications.", Type = "", BadgeColor = "#999", TimeAgo = "" }
                };
                return;
            }

            var mapped = list.Select(n => new
            {
                n.Type,
                BadgeColor = GetBadgeColor(n.Type),
                n.Title,
                Description = n.Message,
                TimeAgo = TimeAgo(n.CreatedAt)
            }).ToList();

            NotifList.ItemsSource = mapped;
        }

        private string GetBadgeColor(string type)
        {
            return type switch
            {
                "BookingApproved" => "#3B82F6",
                "BookingRejected" => "#EF4444",
                "Payment" => "#10B981",
                "ContractReady" => "#6366F1",
                _ => "#6B7280"
            };
        }

        private string TimeAgo(DateTime time)
        {
            var diff = DateTime.UtcNow - time;

            if (diff.TotalMinutes < 1) return "Just now";
            if (diff.TotalHours < 1) return $"{(int)diff.TotalMinutes} minutes ago";
            if (diff.TotalDays < 1) return $"{(int)diff.TotalHours} hours ago";
            return $"{(int)diff.TotalDays} days ago";
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
