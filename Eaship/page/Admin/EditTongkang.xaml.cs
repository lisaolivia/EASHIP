using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class EditTongkang : Page
    {
        private readonly EashipDbContext _context;
        private readonly long _tongkangId;
        private Tongkang? _tongkang;

        public EditTongkang(long tongkangId)
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<EashipDbContext>();
            _tongkangId = tongkangId;

            LoadData();
        }

        private async void LoadData()
        {
            _tongkang = await _context.Tongkangs
                .FirstOrDefaultAsync(t => t.TongkangId == _tongkangId);

            if (_tongkang == null)
            {
                MessageBox.Show("Tongkang not found");
                return;
            }

            NameBox.Text = _tongkang.Name;
            DwtBox.Text = _tongkang.KapasitasDwt;

            StatusBox.ItemsSource = Enum.GetValues(typeof(TongkangStatus));
            StatusBox.SelectedItem = _tongkang.Status;

            IncludeTugboatCheck.IsChecked = _tongkang.IncludeTugboat;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_tongkang == null) return;

            _tongkang.Name = NameBox.Text;
            _tongkang.KapasitasDwt = DwtBox.Text;

            if (StatusBox.SelectedItem is TongkangStatus status)
                _tongkang.SetStatus(status);

            _tongkang.IncludeTugboat = IncludeTugboatCheck.IsChecked ?? false;

            _context.Tongkangs.Update(_tongkang);
            await _context.SaveChangesAsync();

            MessageBox.Show("Tongkang updated successfully!");
            Navigate(new DetailTongkang(_tongkangId));
        }


        private void Navigate(Page page)
        {
            var frame = (Application.Current.MainWindow as MainWindow)?.MainFrame;
            frame?.Navigate(page);
        }

        private void OpenFleetManagement(object s, RoutedEventArgs e) => Navigate(new FleetManagement());
        private void OpenCompanyVerification(object s, RoutedEventArgs e) => Navigate(new CompanyVerification());
        private void OpenBookingRequest(object s, RoutedEventArgs e) => Navigate(new BookingRequest());
        private void OpenContractPayment(object s, RoutedEventArgs e) => Navigate(new ContractPayment());
        private void OpenProfile(object s, RoutedEventArgs e) => Navigate(new Profile());
        private void AddTongkang(object s, RoutedEventArgs e) => Navigate(new TambahTongkang());

    }
}
