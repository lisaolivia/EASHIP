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
            var user = Session.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Anda belum login.");
                return;
            }

            string code = TxtJoinCode.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Join code tidak boleh kosong.");
                return;
            }

            using var scope = App.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EashipDbContext>();


            var company = await db.RenterCompanies
                .FirstOrDefaultAsync(c => c.JoinCode == code);

            if (company == null)
            {
                MessageBox.Show("Join code tidak valid.");
                return;
            }

            // Ambil user dari database
            var dbUser = await db.Users.FirstAsync(u => u.UserId == user.UserId);

            if (dbUser.RenterCompanyId != null)
            {
                MessageBox.Show("Anda sudah tergabung dalam perusahaan.");
                return;
            }

            dbUser.RenterCompanyId = company.RenterCompanyId;
            db.Users.Update(dbUser);
            await db.SaveChangesAsync();

            // refresh session
            Session.Set(dbUser);

            MessageBox.Show("Berhasil bergabung dengan perusahaan!");

            Main?.Navigate(new Dashboard());
        }


    }
}
