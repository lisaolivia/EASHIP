using Eaship.Models;
using Eaship.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eaship.page.Renter
{
    public partial class DetailTongkang : Page
    {
        private readonly EashipDbContext _context;


        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        private Tongkang? _tongkang;

        public DetailTongkang(long tongkangId)
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<EashipDbContext>();

            _tongkang = _context.Tongkangs
                                .FirstOrDefault(t => t.TongkangId == tongkangId);

            if (_tongkang == null)
            {
                MessageBox.Show("Tongkang not found.");
                Main?.Navigate(new Barges());
                return;
            }

            Loaded += DetailTongkang_Loaded;
        }

        private void DetailTongkang_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDetails();
        }

        private void LoadDetails()
        {
            TxtName.Text = _tongkang!.Name;
            TxtStatus.Text = _tongkang!.Status.ToString();
            TxtCapacity.Text = _tongkang!.KapasitasDwt;
            TxtIncludeTug.Text = _tongkang!.IncludeTugboat ? "Yes" : "No";

            var tugCount = _context.TongkangTugboats
                                   .Count(x => x.TongkangId == _tongkang!.TongkangId);

            TxtTugCount.Text = tugCount.ToString();
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new Barges());
        }

        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new BookingPage());
        }

       
    }
}
