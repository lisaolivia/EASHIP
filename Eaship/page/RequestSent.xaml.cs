using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eaship.page
{
    /// <summary>
    /// Interaction logic for RequestSent.xaml
    /// </summary>
    public partial class RequestSent : Page
    {
        private readonly IUserService _users;

        // Shortcut to MainFrame
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public RequestSent()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
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



        // ====== REFRESH BUTTON ======
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new JoinCompanyForm());
        }

        private void Buttonnotifikasi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
