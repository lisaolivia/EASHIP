using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Eaship.Models
{
    // Dipakai design-time (migrations) & bisa dipakai runtime juga
    public class EashipDbContextFactory : IDesignTimeDbContextFactory<EashipDbContext>
    {
        public EashipDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            // kalau dipanggil dari Tools, basePath = folder project
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var cs = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<EashipDbContext>()
                .UseNpgsql(cs)
                .Options;

            return new EashipDbContext(options);
        }
    }
}
