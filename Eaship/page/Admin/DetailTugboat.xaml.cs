using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eaship.page.Admin
{
    public partial class DetailTugboat : Page
    {
        private readonly EashipDbContext _context;
        private Tugboat? _tugboat;
        private readonly long _id;

        public DetailTugboat(long id)
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            _id = id;

            Loaded += DetailTugboat_Loaded;
        }

        private async void DetailTugboat_Loaded(object sender, RoutedEventArgs e)
        {
            _tugboat = await _context.Tugboats.FirstOrDefaultAsync(t => t.TugboatId == _id);

            if (_tugboat == null)
            {
                MessageBox.Show("Tugboat tidak ditemukan.");
                Navigate(new TugboatListAdmin());
                return;
            }

            // Fill UI
            NameText.Text = _tugboat.Nama;
            HpText.Text = _tugboat.TugboatHp;
            StatusText.Text = _tugboat.Status.ToString();

            // Load image with fallback
            TugboatImage.Source = new BitmapImage(
                new Uri("pack://application:,,,/Eaship;component/Assets/tugboat_default.jpeg")
            );

            // Tongkang relation info: sementara
            TongkangInfo.Text = "Belum terhubung dengan tongkang.";
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new EditTugboat(_id));
        }

        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        // Navbar
        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());
    }
}
