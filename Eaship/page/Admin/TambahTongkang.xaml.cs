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
    /// Interaction logic for TambahTongkang.xaml
    /// </summary>
    public partial class TambahTongkang : Page
    {
        private readonly EashipDbContext _context;

        public TambahTongkang()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var tongkang = new Tongkang
            {
                Name = NameBox.Text,
                KapasitasDwt = KapasitasBox.Text,
                // Status optional, kalau ada StatusBox
            };

            _context.Tongkangs.Add(tongkang);
            await _context.SaveChangesAsync();

            MessageBox.Show("Tongkang added!");

            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new TongkangListAdmin());
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
