using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class CompanyVerification : Page
    {
        private readonly ICompanyService _companies;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public CompanyVerification()
        {
            InitializeComponent();
            _companies = App.Services.GetRequiredService<ICompanyService>();

            Loaded += async (_, __) => await LoadData();
        
        }

        private void Navigate(Page page)
        {
            Main?.Navigate(page);
        }

        // NAVBAR
        private void OpenFleetManagement(object s, RoutedEventArgs e)
            => Navigate(new FleetManagement());

        private void OpenCompanyVerification(object s, RoutedEventArgs e)
        {
            MessageBox.Show("You are already on Company Verification page.",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenBookingRequest(object s, RoutedEventArgs e)
            => Navigate(new BookingRequest());

        private void OpenContractPayment(object s, RoutedEventArgs e)
            => Navigate(new ContractPayment());

        private void OpenProfile(object s, RoutedEventArgs e)
            => Navigate(new Profile());

        // SEE MORE BAWAH (List Company)
        private void SeeMore_Click(object sender, RoutedEventArgs e)
            => Navigate(new ListCompany());

        // SEE MORE ATAS (Pending Detail)
        private void OpenPendingDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
                Navigate(new CompanyVerificationDetail(id));
        }

        // LOAD DATA
        private async Task LoadData()
        {
            var pending = await _companies.GetPendingAsync();
            PendingList.ItemsSource = pending;

            var approved = await _companies.GetApprovedAsync();
            txtCompanyVerified.Text = approved.Count.ToString();
        }

    }
}
