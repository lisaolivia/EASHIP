using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Eaship.page.Admin
{
    public partial class CompanyVerification : Page
    {
        private readonly ICompanyService _companies;

        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;

        public CompanyVerification()
        {
            InitializeComponent();
            _companies = App.Services.GetRequiredService<ICompanyService>();

            Loaded += async (_, __) => await LoadData();
        
        }

        private void Navigate(Page page)
        {
            Main?.Navigate(page);
        }


        // SEE MORE BAWAH (List Company)
        private void SeeMore_Click(object sender, RoutedEventArgs e)
            => Navigate(new ListCompany());

        // SEE MORE ATAS (Pending Detail)
        private void OpenPendingDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
                Navigate(new CompanyVerificationDetail(id));
        }

        // LOAD DATA
        private async Task LoadData()
        {
            var pending = await _companies.GetPendingAsync();
            PendingList.ItemsSource = pending;

            var approved = await _companies.GetApprovedAsync();
            txtCompanyVerified.Text = approved.Count.ToString();
        }

    }
}
