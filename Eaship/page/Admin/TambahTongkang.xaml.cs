using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eaship.page.Admin
{
    public partial class TambahTongkang : Page
    {
        private readonly EashipDbContext _context;

        private string? PhotoUrl = null;   // <== VARIABLE WAJIB

        public TambahTongkang()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
        }

        private void UploadFoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                // preview
                PreviewFoto.Source = new BitmapImage(new Uri(dialog.FileName));
                PreviewFoto.Visibility = Visibility.Visible;

                var cloud = new CloudinaryService();
                PhotoUrl = cloud.UploadImage(dialog.FileName); // <=== FIX

                MessageBox.Show("Foto berhasil diupload!");
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var tongkang = new Tongkang
            {
                Name = NameBox.Text,
                KapasitasDwt = KapasitasBox.Text,
                PhotoUrl = PhotoUrl,                   // <== SIMPAN URL
                IncludeTugboat = false,
                // Status default = Available (private setter)
            };

            _context.Tongkangs.Add(tongkang);
            await _context.SaveChangesAsync();

            MessageBox.Show("Tongkang added!");

            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new TongkangListAdmin());
        }
    }
}
