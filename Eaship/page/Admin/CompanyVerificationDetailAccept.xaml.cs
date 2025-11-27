using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class CompanyVerificationDetailAccept : Page
    {
        private readonly ICompanyService _companies;
        private readonly int _companyId;

        public CompanyVerificationDetailAccept(int companyId)
        {
            InitializeComponent();

            _companies = App.Services.GetRequiredService<ICompanyService>();
            _companyId = companyId;
        }


        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            await _companies.ApproveAsync(_companyId, Session.CurrentUser);

            var db = App.Services.GetRequiredService<EashipDbContext>();
            var renter = await db.Users.FirstAsync(u => u.RenterCompanyId == _companyId);
            Session.Set(renter);   //  refresh user renter
            await Session.RefreshAsync(db);
            MessageBox.Show("Company approved!");
            Navigate(new CompanyVerification());

        }

        private async void Decline_Click(object sender, RoutedEventArgs e)
        {
            string reason = TxtReason.Text.Trim();  

            await _companies.RejectAsync(_companyId, Session.CurrentUser, reason);

            var db = App.Services.GetRequiredService<EashipDbContext>();
            var renter = await db.Users.FirstAsync(u => u.RenterCompanyId == _companyId);
            Session.Set(renter);   // <-- refresh renter

            MessageBox.Show("Company declined!");
            Navigate(new CompanyVerification());
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
