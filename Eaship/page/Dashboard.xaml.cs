using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Eaship.page
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private readonly IUserService _users;
        private readonly EashipDbContext _context;
        private User? _currentUser;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        // untuk simpan user aktif

        public Dashboard()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
            _context = App.Services.GetRequiredService<EashipDbContext>(); // ambil user aktif dari session
            Loaded += Dashboard_Loaded;
            _currentUser = Session.CurrentUser;
            
        }

        // ====== NAVBAR: Barges ======
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            MessageBox.Show("Barges diklik! (stub sementara)");
        }

        // ====== NAVBAR: My Bookings ======
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                Main?.Navigate(new RequireLoginPage());
                return;
            }

            MessageBox.Show("My Bookings diklik! (stub sementara)");
        }

        // ====== NAVBAR: Help ======
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help diklik! (coming soon)");
        }


        private void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                _currentUser = Session.CurrentUser; // ambil dari session langsung
                if (_currentUser == null)
                {
                    MessageBox.Show("Anda belum login. Silakan login terlebih dahulu.");
                    Main?.Navigate(new RequireLoginPage());
                    return;
                }
            }


            var company = _context.RenterCompanies
                .FirstOrDefault(c => c.CreatedBy == _currentUser.UserId);

            if (company == null)
            {
                SectionWelcome.Visibility = Visibility.Visible;
                SectionWaiting.Visibility = Visibility.Collapsed;
            }
            else if (company.Status == CompanyStatus.Validating)
            {
                SectionWelcome.Visibility = Visibility.Collapsed;
                SectionWaiting.Visibility = Visibility.Visible;

                MessageBox.Show("DEBUG: SectionWaiting Visible!");
            }
            else
            {
                SectionWelcome.Visibility = Visibility.Collapsed;
                SectionWaiting.Visibility = Visibility.Collapsed;
            }

        }



        private void BtnCreateCompany_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new CompanyFormPage());
        }

        private void BtnJoinExisting_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new RequestSent());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }
    }
}
