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
    public partial class TambahTugboat : Page
    {
        private readonly EashipDbContext _context;
        private string? PhotoUrl;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public TambahTugboat()
        {
            InitializeComponent();

            // PENTING BANGET
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
                PreviewFoto.Source = new BitmapImage(new Uri(dialog.FileName));
                PreviewFoto.Visibility = Visibility.Visible;

                var cloud = new CloudinaryService();
                PhotoUrl = cloud.UploadImage(dialog.FileName);

                MessageBox.Show("Foto tugboat berhasil diupload!");
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Nama tugboat harus diisi.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtHp.Text))
            {
                MessageBox.Show("HP harus diisi.");
                return;
            }

            var statusStr = (ComboStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "AVAILABLE";
            var statusEnum = Enum.Parse<TugboatStatus>(statusStr.ToUpper());

            var tug = new Tugboat
            {
                Nama = TxtName.Text.Trim(),
                TugboatHp = TxtHp.Text.Trim(),
                PhotoUrl = PhotoUrl,
                Status = statusEnum
            };

            // SIMPAN KE DATABASE
            _context.Tugboats.Add(tug);
            await _context.SaveChangesAsync();

            MessageBox.Show("Tugboat berhasil ditambahkan!");

            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(new TugboatListAdmin());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => Main?.Navigate(new Eaship.page.Admin.TugboatListAdmin());


    }
}
