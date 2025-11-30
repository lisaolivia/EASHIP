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
        private readonly INotificationService _notif;


        public CompanyVerificationDetail(int companyId)
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _companies = App.Services.GetRequiredService<ICompanyService>();
            _notif = App.Services.GetRequiredService<INotificationService>();
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

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            // 1. Approve company
            await _companies.ApproveAsync(_companyId, Session.CurrentUser);

            // 2. Get renter user who owns this company
            var db = App.Services.GetRequiredService<EashipDbContext>();
            var renter = await db.Users.FirstAsync(u => u.RenterCompanyId == _companyId);

            // 3. Create notification
            _notif.Create(
                renter.UserId,
                "CompanyApproved",
                "Company Approved",
                "Your company has been approved and is now active!",
                companyId: renter.RenterCompanyId
            );

            MessageBox.Show("Company approved!");

            // 4. Return to list
            Navigate(new CompanyVerification());
        }

        private async void Decline_Click(object sender, RoutedEventArgs e)
        {
            string reason = TxtReason.Text.Trim();

            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Please enter a reason.");
                return;
            }

            // 1. Reject company
            await _companies.RejectAsync(_companyId, Session.CurrentUser, reason);

            // 2. Get renter (company owner)
            var db = App.Services.GetRequiredService<EashipDbContext>();
            var renter = await db.Users.FirstAsync(u => u.RenterCompanyId == _companyId);

            // 3. Create notification
            _notif.Create(
                renter.UserId,
                "CompanyRejected",
                "Company Rejected",
                $"Your company request was rejected.\nReason: {reason}",
                companyId: renter.RenterCompanyId
            );

            MessageBox.Show("Company declined!");
            Navigate(new CompanyVerification());
        }

        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }
    }
}
