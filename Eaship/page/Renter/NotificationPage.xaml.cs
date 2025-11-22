using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class NotificationPage : Page
    {
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public NotificationPage()
        {
            InitializeComponent();
            Loaded += NotificationPage_Loaded;
        }

        private void NotificationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            // MOCK DATA SEMENTARA
            var list = new List<object>()
            {
                new {
                    Type = "Booking Approved",
                    BadgeColor = "#3B82F6",
                    Title = "Your Tongkang Booking Has Been Approved",
                    Description = "Admin has approved your booking request for MBSS T-308.",
                    TimeAgo = "2 hours ago"
                },
                new {
                    Type = "Payment",
                    BadgeColor = "#10B981",
                    Title = "Invoice Paid Successfully",
                    Description = "Your payment for Invoice INV-2025-091 has been received.",
                    TimeAgo = "1 day ago"
                },
                new {
                    Type = "Warning",
                    BadgeColor = "#EF4444",
                    Title = "Upcoming Due Date",
                    Description = "Your contract payment will be due in 3 days. Please prepare payment.",
                    TimeAgo = "3 days ago"
                },
                new {
                    Type = "Info",
                    BadgeColor = "#6366F1",
                    Title = "System Maintenance",
                    Description = "The system will undergo maintenance tonight from 22:00 - 02:00.",
                    TimeAgo = "5 days ago"
                }
            };

            NotifList.ItemsSource = list;
        }


        // NAVBAR HANDLER
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
