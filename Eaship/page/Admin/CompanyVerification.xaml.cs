using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using Eaship.Models;

namespace Eaship.page.Admin
{
    public partial class CompanyVerification : Page
    {
        private readonly ICompanyService _companies;

        // Akses MainFrame global
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public CompanyVerification()
        {
            InitializeComponent();
            _companies = App.Services.GetRequiredService<ICompanyService>();
        }

        // =============================
        // GENERIC NAVIGATION METHOD
        // =============================
        private void Navigate(Page page)
        {
            Main?.Navigate(page);
        }

        // =============================
        // NAVBAR BUTTONS
        // =============================

        private void OpenFleetManagement(object s, RoutedEventArgs e)
            => Navigate(new FleetManagement());

        // ⛔ FIX: jangan navigate ke dirinya sendiri
        private void OpenCompanyVerification(object s, RoutedEventArgs e)
        {
            // optional: msgbox biar user tahu dia sudah di halaman ini
            MessageBox.Show("You are already on Company Verification page.",
                            "Info",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void OpenBookingRequest(object s, RoutedEventArgs e)
            => Navigate(new BookingRequest());

        private void OpenContractPayment(object s, RoutedEventArgs e)
            => Navigate(new ContractPayment());

        private void OpenProfile(object s, RoutedEventArgs e)
            => Navigate(new Profile());

        // =============================
        // INSIDE PAGE BUTTONS
        // =============================

        private void SeeMore_Click(object sender, RoutedEventArgs e)
            => Navigate(new ListCompany());

        private void AddTongkang(object s, RoutedEventArgs e)
            => Navigate(new TambahTongkang());

        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }
    }
}
