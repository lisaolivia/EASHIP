using Eaship.Models;
using Eaship.Services;   // <--- WAJIB UNTUK AKSES Session
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class Profile : Page
    {
        private readonly EashipDbContext _context;
        private User? _currentUser;

        public Profile()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            LoadUser();
        }

        private void LoadUser()
        {
            // Ambil user admin dari session
            _currentUser = Session.CurrentUser;   // <--- FIXED

            if (_currentUser == null)
            {
                MessageBox.Show("User not found");
                return;
            }

            NameBox.Text = _currentUser.FullName;
            EmailBox.Text = _currentUser.Email;
            PhoneBox.Text = _currentUser.Phone;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null) return;

            _currentUser.FullName = NameBox.Text;
            _currentUser.Phone = PhoneBox.Text;

            _context.Users.Update(_currentUser);
            await _context.SaveChangesAsync();

            // Update session user setelah perubahan
            Session.Set(_currentUser);  // <--- FIXED

            MessageBox.Show("Profile updated!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new DashboardAdmin());
        }

        // NAVIGASI SIDEBAR
        private void Navigate(Page p)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(p);
        }

        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }
    }
}
