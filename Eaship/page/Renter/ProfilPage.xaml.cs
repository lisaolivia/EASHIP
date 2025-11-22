using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Renter
{
    public partial class ProfilPage : Page
    {
        private readonly EashipDbContext _context;
        private readonly User _user;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;


        public ProfilPage()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();
            _user = Session.CurrentUser!;

            Loaded += ProfilPage_Loaded;
        }

        private void ProfilPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserInfo();
            LoadCompanyInfo();
        }

        private void LoadUserInfo()
        {
            TxtFullName.Text = _user.FullName;
            TxtEmail.Text = _user.Email;
            TxtPhone.Text = _user.Phone;
            TxtRole.Text = _user.Role.ToString();
            TxtLastLogin.Text = _user.LastLoginAt?.ToString("dd MMM yyyy HH:mm") ?? "-";
        }

        private void LoadCompanyInfo()
        {
            // No company yet
            if (_user.RenterCompanyId == null)
            {
                CompanySection.Visibility = Visibility.Collapsed;
                NoCompanySection.Visibility = Visibility.Visible;
                return;
            }

            var company = _context.RenterCompanies
                .FirstOrDefault(c => c.RenterCompanyId == _user.RenterCompanyId);

            if (company == null)
            {
                CompanySection.Visibility = Visibility.Collapsed;
                NoCompanySection.Visibility = Visibility.Visible;
                return;
            }

            CompanySection.Visibility = Visibility.Visible;
            NoCompanySection.Visibility = Visibility.Collapsed;

            TxtCompanyName.Text = company.Nama;
            TxtNPWP.Text = company.NPWP;
            TxtAddress.Text = company.Address;
            TxtCityProvince.Text = company.CityProvince;
            TxtPICName.Text = company.PICName;
            TxtPICPosition.Text = company.PICPosition;
            TxtPICEmail.Text = company.PICEmail;
            TxtCompanyStatus.Text = company.Status.ToString();

            // Badge color based on status
            StatusBadge.Background = company.Status switch
            {
                CompanyStatus.Active => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green),
                CompanyStatus.Validating => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange),
                CompanyStatus.Rejected => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red),
                _ => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray)
            };
        }


        // NAVIGATION
        private void BtnBarges_Click(object sender, RoutedEventArgs e) => Main?.Navigate(new Barges());
        private void BtnMyBookings_Click(object sender, RoutedEventArgs e) => Main?.Navigate(new MyBookingPage());
        private void BtnContract_Click(object sender, RoutedEventArgs e) => Main?.Navigate(new ContractPage());
        private void BtnNotif_Click(object sender, RoutedEventArgs e) => Main?.Navigate(new NotificationPage());
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }

        private void BtnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new CompanyFormPage());
        }

        private void BtnJoinCompany_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new JoinCompanyForm());
        }
    }
}
