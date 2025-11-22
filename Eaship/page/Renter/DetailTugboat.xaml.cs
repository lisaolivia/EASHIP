using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eaship.page.Renter
{
    public partial class DetailTugboat : Page
    {
        private readonly EashipDbContext _context;
        private readonly Tugboat _tugboat;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        public DetailTugboat(Tugboat tugboat)
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();

            _tugboat = tugboat;
            Loaded += DetailTugboat_Loaded;
        }

        private void DetailTugboat_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            TxtName!.Text = _tugboat.Nama;
            TxtStatus!.Text = _tugboat.Status.ToString();
            TxtHp!.Text = _tugboat.TugboatHp;
            TxtStatusDetail!.Text = _tugboat.Status.ToString();

            var assigned = _context.TongkangTugboats
                .Where(x => x.TugboatId == _tugboat.TugboatId)
                .Select(x => x.Tongkang!.Name)
                .FirstOrDefault();

            TxtAssigned!.Text = assigned ?? "Not Assigned";

            TugboatImage!.Source = new BitmapImage(
                new Uri("pack://application:,,,/Eaship;component/Assets/default_tugboat.png")
            );
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new Barges());
        }

        private void GoBarges(object sender, RoutedEventArgs e)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new Barges());
        }

        //===================== NAVIGATION =====================


        private void BtnBarges_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new Barges());
        }

        private void BtnMyBookings_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new MyBookingPage());
        }

        private void BtnContract_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ContractPage());
        }

        private void BtnNotif_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new NotificationPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ProfilPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Clear();
            Main?.Navigate(new LogoutPage());
        }

        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new BookingPage());
        }
    }
}
