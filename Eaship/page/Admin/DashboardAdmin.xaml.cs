using Eaship.Models;
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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Eaship.page.Admin
{
    /// <summary>
    /// Interaction logic for DashboardAdmin.xaml
    /// </summary>
    //private readonly ICompanyService _companies;
    public partial class DashboardAdmin : Page // Ensure this matches the base class in all partial declarations
    {
        private readonly ICompanyService _companies;

        public DashboardAdmin()
        {
            InitializeComponent();
            _companies = App.Services.GetRequiredService<ICompanyService>();

            Loaded += DashboardAdmin_Loaded;
        }

        private async void DashboardAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            // load data
            var pending = await _companies.GetPendingAsync();
            var active = await _companies.GetActiveAsync();

            // update cards
            txtCompanyVerified.Text = active.Count.ToString();

            // update notifications
            NotificationList.ItemsSource = pending.Select(c => new
            {
                Title = "Company Verification",
                Company = c.Nama,
                NPWP = c.NPWP,
                Date = c.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
            });
        }


        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());
        private void EditTongkang(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTongkang(id));
        }

    }
}