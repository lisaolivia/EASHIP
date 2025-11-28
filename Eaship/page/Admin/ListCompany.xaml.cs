using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Eaship.Models;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eaship.page.Admin
{
    /// <summary>
    /// Interaction logic for ListCompany.xaml
    /// </summary>
    public partial class ListCompany : Page
    {

        private readonly EashipDbContext _context;
        public ListCompany()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            Loaded += ListCompany_Loaded;
        }

        private async void ListCompany_Loaded(object sender, RoutedEventArgs e)
        {
            var companies = await _context.RenterCompanies.ToListAsync();
            CompanyTable.ItemsSource = companies;

        }

        private void OpenDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
                Navigate(new CompanyVerificationDetail(id));
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
