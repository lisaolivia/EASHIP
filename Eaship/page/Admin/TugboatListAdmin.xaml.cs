using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class TugboatListAdmin : Page
    {
        private readonly EashipDbContext _context;

        public TugboatListAdmin()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            Loaded += TugboatListAdmin_Loaded;
        }

        private async void TugboatListAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            var tugboats = await _context.Tugboats.ToListAsync();
            TugboatList.ItemsSource = tugboats;
        }

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        private void Navigate(Page page) => Main?.Navigate(page);

        private void AddTugboat(object s, RoutedEventArgs e)
        {
            Navigate(new TambahTugboat());
        }

        private void EditTugboat(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTugboat(id));
        }

        // NAVBAR
        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Navigate(new LogoutPage());
        }
    }
}
