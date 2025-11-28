using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Controls
{
    public partial class RenterNavbar : UserControl
    {
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RenterNavbar()
        {
            InitializeComponent();
        }

        private void Navigate(Page page) => Main?.Navigate(page);

        private void BtnGoDashboard_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.Dashboard());

        private void BtnBarges_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.Barges());

        private void BtnMyBookings_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.MyBookingPage());

        private void BtnContract_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.ContractPage());

        private void BtnNotif_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.NotificationPage());

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.Renter.ProfilPage());

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
            => Navigate(new Eaship.page.LogoutPage());
    }
}
