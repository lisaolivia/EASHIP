using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Controls
{
    public partial class AdminNavbar : UserControl
    {
        public AdminNavbar()
        {
            InitializeComponent();
        }

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        private void GoDashboard(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.DashboardAdmin());

        private void GoFleetManagement(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.FleetManagement());

        private void GoCompanyVerification(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.CompanyVerification());

        private void GoBookingRequest(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.BookingRequest());

        private void GoContractPayment(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.ContractPayment());

        private void GoProfile(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.Admin.Profile());

        private void GoLogout(object s, RoutedEventArgs e)
            => Main?.Navigate(new page.LogoutPage());
    }
}
