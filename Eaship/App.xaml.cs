using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using Eaship.Models;
using Eaship.Services;

namespace Eaship
{
    public partial class App : Application
    {
        public static ServiceProvider Services { get; private set; } = default!;
        public static User? CurrentUser { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(config);

            sc.AddDbContext<EashipDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            // REGISTER ALL SERVICES DI SINI
            sc.AddScoped<IUserService, UserService>();
            sc.AddScoped<ICompanyService, CompanyService>();
            sc.AddScoped<ITugboatService, TugboatService>();
            sc.AddScoped<INotificationService, NotificationService>();



            Services = sc.BuildServiceProvider();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EashipDbContext>();
            db.Database.EnsureCreated();
        }
    }
}
