using Eaship.Models;
using Eaship.page.Renter;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Eaship.page.Admin
{
    public partial class TugboatListAdmin : Page
    {
        private readonly EashipDbContext _context;

        public TugboatListAdmin()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            Loaded += TugboatListAdmin_Loaded;
        }

        private async void TugboatListAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            var tugboats = await _context.Tugboats.ToListAsync();
            TugboatList.ItemsSource = tugboats;
        }

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        private void Navigate(Page page) => Main?.Navigate(page);

        private void AddTugboat(object s, RoutedEventArgs e)
        {
            Navigate(new TambahTugboat());
        }

        private void EditTugboat(object s, RoutedEventArgs e)
        {
            if (s is Button b && b.Tag is long id)
                Navigate(new EditTugboat(id));
        }

        // NAVBAR
     
        private void OpenDetailTugboat(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Tugboat t)
                Navigate(new DetailTugboat(t.TugboatId));
        }

    }
}
