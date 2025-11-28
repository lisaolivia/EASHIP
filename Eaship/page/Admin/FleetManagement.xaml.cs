using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class FleetManagement : Page
    {
        private readonly EashipDbContext _context;

        public FleetManagement()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            LoadData();
        }

        private async void LoadData()
        {
            // Ambil total tongkang & tugboat
            int tongkangCount = await _context.Tongkangs.CountAsync();
            int tugboatCount = await _context.Tugboats.CountAsync();

            // Tampilkan ke UI
            TongkangCountText.Text = tongkangCount.ToString();
            TugboatCountText.Text = tugboatCount.ToString();
        }

        // ====================== NAVIGATION ======================
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

        private void SeeMoreTongkang(object sender, RoutedEventArgs e)
        {
            Navigate(new TongkangListAdmin());
        }

        private void SeeMoreTugboat(object sender, RoutedEventArgs e)
        {
            Navigate(new TugboatListAdmin()); 
        }

    }
}
