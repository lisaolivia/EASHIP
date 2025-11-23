using Eaship.Models;
using Microsoft.EntityFrameworkCore;
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

namespace Eaship.page.Admin
{
    /// <summary>
    /// Interaction logic for TongkangListAdmin.xaml
    /// </summary>
    public partial class TongkangListAdmin : Page
    {
        private readonly EashipDbContext _context;

        public TongkangListAdmin()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            LoadData();
        }

        private async void LoadData()
        {
            var data = await _context.Tongkangs.ToListAsync();
            TongkangList.ItemsSource = data;
        }

        private void Navigate(Page p)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(p);
        }


        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());

        private void OpenDetailTongkang(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Tongkang t)
                Navigate(new DetailTongkang(t.TongkangId));
        }




    }
}
