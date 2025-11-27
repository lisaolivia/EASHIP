using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class CompanyVerificationDetail : Page
    {
        private readonly EashipDbContext _context;
        private readonly int _companyId;
        private RenterCompany? _company;
        private readonly ICompanyService _companies;


        public CompanyVerificationDetail(int companyId)
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _companyId = companyId;

            LoadData();
        }




        private async void LoadData()
        {
            _company = await _context.RenterCompanies
                .FirstOrDefaultAsync(c => c.RenterCompanyId == _companyId);

            if (_company == null)
            {
                MessageBox.Show("Company not found!");
                return;
            }

            // Bind Data into UI
            CompanyNameBox.Text = _company.Nama;
            NPWPBox.Text = _company.NPWP;
            AddressBox.Text = _company.Address;
            CityProvinceBox.Text = _company.CityProvince;
            EmailBillingBox.Text = _company.EmailBilling;
            PhoneBox.Text = _company.PhoneNumber;

            PICNameBox.Text = _company.PICName;
            PICPositionBox.Text = _company.PICPosition;
            PICEmailBox.Text = _company.PICEmail;
        }

        // NAVIGATION
        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
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

        private void GoToVerification_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new CompanyVerificationDetailAccept(_companyId));
        }
    }
}
