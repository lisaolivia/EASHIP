using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class BookingPage : Page
    {
        private readonly EashipDbContext _context;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public BookingPage()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string role = (RoleBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string name = TxtFullName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string phone = TxtPhone.Text.Trim();
            string notes = TxtNotes.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Name & Email must be filled.");
                return;
            }

            MessageBox.Show($"Booking submitted!\n\nName: {name}\nEmail: {email}\nPhone: {phone}\nRole: {role}");
        }

        // ================= NAVBAR =================

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
