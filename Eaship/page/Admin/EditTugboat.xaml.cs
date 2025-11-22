using Eaship.Models;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class EditTugboat : Page
    {
        private readonly EashipDbContext _context;
        private Tugboat _tugboat;
        private long _id;


        public EditTugboat(long id)   // <<=== INI constructor yg dibutuhin
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            _id = id;

            Loaded += EditTugboat_Loaded;
        }

        private async void EditTugboat_Loaded(object sender, RoutedEventArgs e)
        {
            _tugboat = await _context.Tugboats.FirstOrDefaultAsync(t => t.TugboatId == _id);

            if (_tugboat == null)
            {
                MessageBox.Show("Tugboat tidak ditemukan.");
                Main?.Navigate(new TugboatListAdmin());
                return;
            }

            // isi ke textbox
            TxtName.Text = _tugboat.Nama;
            TxtHp.Text = _tugboat.TugboatHp;
            ComboStatus.SelectedItem = _tugboat.Status.ToString();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_tugboat == null) return;

            _tugboat.Nama = TxtName.Text.Trim();
            _tugboat.TugboatHp = TxtHp.Text.Trim();

            if (ComboStatus.SelectedItem != null)
                _tugboat.Status = Enum.Parse<TugboatStatus>(ComboStatus.SelectedItem.ToString());

            await _context.SaveChangesAsync();

            MessageBox.Show("Tugboat updated successfully!");
            Main?.Navigate(new TugboatListAdmin());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new TugboatListAdmin());
        }

        // ==================== NAVIGATION ====================

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        private void OpenFleetManagement(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new FleetManagement());
        }

        private void OpenCompanyVerification(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new CompanyVerification());
        }

        private void OpenBookingRequest(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new BookingRequest());
        }

        private void OpenContractPayment(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new ContractPayment());
        }

        private void OpenProfile(object sender, RoutedEventArgs e)
        {
            Main?.Navigate(new Profile());
        }

    }
}
