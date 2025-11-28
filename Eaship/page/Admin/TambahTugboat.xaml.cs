using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class TambahTugboat : Page
    {
        private readonly ITugboatService _tugboats;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public TambahTugboat()
        {
            InitializeComponent();
            _tugboats = App.Services.GetRequiredService<ITugboatService>();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new TugboatListAdmin());
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

            var status = (ComboStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Available";

            var tug = new Tugboat
            {
                Nama = TxtName.Text.Trim(),
                TugboatHp = TxtHp.Text.Trim(),
                Status = Enum.Parse<TugboatStatus>(status)
            };

            await _tugboats.AddAsync(tug);

            MessageBox.Show("Tugboat berhasil ditambahkan!");
            Main?.Navigate(new TugboatListAdmin());
        }

        // NAVIGATION
      
    }
}
