using Eaship.page.Renter;
using Eaship.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eaship.Models;
using Microsoft.EntityFrameworkCore;

namespace Eaship.page
{
    /// <summary>
    /// Interaction logic for JoinCompanyForm.xaml
    /// </summary>
    public partial class JoinCompanyForm : Page
    {
        private readonly IUserService _users;
        private Frame? Main => (Application.Current.MainWindow as MainWindow)?.MainFrame;
        public JoinCompanyForm()
        {
            InitializeComponent();
            _users = App.Services.GetRequiredService<IUserService>();
        }

        private async void JoinCompany_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Session.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Anda belum login!");
                return;
            }

            // Validasi input company ID
            if (!int.TryParse(TxtCompanyId.Text, out int companyId))
            {
                MessageBox.Show("Company ID tidak valid.");
                return;
            }

            // Ambil DbContext
            var db = App.Services.GetRequiredService<EashipDbContext>();

            // Cari company berdasarkan ID
            var company = await db.RenterCompanies
                .FirstOrDefaultAsync(c => c.RenterCompanyId == companyId);

            if (company == null)
            {
                MessageBox.Show("Perusahaan tidak ditemukan.");
                return;
            }

            // Set relasi user → company
            currentUser.RenterCompanyId = companyId;

            // Simpan ke database
            await db.SaveChangesAsync();

            await Session.RefreshAsync(db);


            // Refresh session user (WAJIB)
            var freshUser = await db.Users
                .FirstAsync(u => u.UserId == currentUser.UserId);
            Session.Set(freshUser);

            MessageBox.Show("Berhasil join perusahaan! Menunggu verifikasi admin.");

            // Navigate ke Dashboard (renter)
            Main?.Navigate(new Dashboard());
        }

    }
}
