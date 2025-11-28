using Eaship.Models;
using Microsoft.EntityFrameworkCore;

namespace Eaship.Models
{
    public class EashipDbContext : DbContext
    {
        public EashipDbContext(DbContextOptions<EashipDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<RenterCompany> RenterCompanies => Set<RenterCompany>();

        public DbSet<Tongkang> Tongkangs => Set<Tongkang>();
        public DbSet<Tugboat> Tugboats => Set<Tugboat>();
        public DbSet<BookingTongkang> BookingTongkangs => Set<BookingTongkang>();

        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<TongkangTugboat> TongkangTugboats => Set<TongkangTugboat>();
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Schema setup
            modelBuilder.HasDefaultSchema("eashipp");
            modelBuilder.HasPostgresExtension("citext");
            modelBuilder.HasPostgresEnum<UserRole>("eaship", "user_role");

            // ========================
            // USER
            // ========================
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users", "eaship");
                e.HasKey(x => x.UserId);

                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.RenterCompanyId).HasColumnName("renter_company_id");

                e.Property(x => x.Email).HasColumnName("email").HasColumnType("citext");
                e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(250);
                e.Property(x => x.PasswordHash).HasColumnName("password_hash");
                e.Property(x => x.PasswordSalt).HasColumnName("password_salt");
                e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(50);

                e.Property(x => x.Role).HasColumnName("role").HasConversion<string>();
                e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");

                e.HasOne(u => u.RenterCompany).WithMany().HasForeignKey(u => u.RenterCompanyId);


            });



            // ========================
            // TUGBOAT
            // ========================
            modelBuilder.Entity<Tugboat>(e =>
            {
                e.ToTable("tugboat", "eaship");
                e.HasKey(x => x.TugboatId);

                e.Property(x => x.TugboatId).HasColumnName("tugboat_id");
                e.Property(x => x.Nama).HasColumnName("nama");
                e.Property(x => x.TugboatHp).HasColumnName("tugboat_hp");
                e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();
              
            });

            // ========================
            // TONGKANG
            // ========================
           
            modelBuilder.Entity<Tongkang>(e =>
            {
                e.ToTable("tongkang", "eaship");
                e.HasKey(x => x.TongkangId);

                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");
                e.Property(x => x.Name).HasColumnName("name");
                e.Property(x => x.KapasitasDwt).HasColumnName("kapasitas_dwt");
    
                e.Property(x => x.IncludeTugboat).HasColumnName("include_tugboat");
                e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();

            });


            // ========================
            // MANY-TO-MANY: TONGKANG <-> TUGBOAT
            // ========================
            modelBuilder.Entity<TongkangTugboat>(e =>
            {
                e.ToTable("tongkang_tugboat", "eaship");
                e.HasKey(x => new { x.TongkangId, x.TugboatId });

                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");
                e.Property(x => x.TugboatId).HasColumnName("tugboat_id");

                e.HasOne(x => x.Tongkang)
                    .WithMany()
                    .HasForeignKey(x => x.TongkangId);

                e.HasOne(x => x.Tugboat)
                    .WithMany()
                    .HasForeignKey(x => x.TugboatId);
            });

            // ========================
            // BOOKING
            // ========================
            modelBuilder.Entity<Booking>(e =>
            {
                e.ToTable("booking", "eaship");
                e.HasKey(b => b.BookingId);

                e.Property(b => b.BookingId).HasColumnName("booking_id");
                e.Property(b => b.UserId).HasColumnName("user_id");
                e.Property(b => b.OriginPort).HasColumnName("origin_port");
                e.Property(b => b.DestinationPort).HasColumnName("destination_port");
                e.Property(b => b.StartDate).HasColumnName("start_date");
                e.Property(b => b.DurationDays).HasColumnName("duration_days");
                e.Property(b => b.CargoDesc).HasColumnName("cargo_desc");
                e.Property(b => b.HargaTotal).HasColumnName("harga_total").HasColumnType("numeric(18,2)");
                e.Property(b => b.Status).HasColumnName("status").HasConversion<string>();
                e.Property(b => b.CreatedAt).HasColumnName("created_at");

                    e.HasOne(b => b.User)
                    .WithMany()
                    .HasForeignKey(b => b.UserId);
            });

            // ========================
            // MANY-TO-MANY: BOOKING <-> TONGKANG
            // ========================
            modelBuilder.Entity<BookingTongkang>(e =>
            {
                e.ToTable("booking_tongkang", "eaship");
                e.HasKey(x => new { x.BookingId, x.TongkangId });

                e.Property(x => x.BookingId).HasColumnName("booking_id");
                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");

                e.HasOne(x => x.Booking)
                    .WithMany(b => b.BookingTongkangs)
                    .HasForeignKey(x => x.BookingId);

                e.HasOne(x => x.Tongkang)
                    .WithMany()
                    .HasForeignKey(x => x.TongkangId);
            });

            // ========================
            // CONTRACT
            // ========================
            modelBuilder.Entity<Contract>(e =>
            {
                e.ToTable("contract", "eaship");
                e.HasKey(x => x.ContractId);

                e.Property(x => x.ContractId).HasColumnName("contract_id");
                e.Property(x => x.BookingId).HasColumnName("booking_id");
                e.Property(x => x.PdfUrl).HasColumnName("pdf_url");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
              

            });

            // ========================
            // INVOICE
            // ========================
            modelBuilder.Entity<Invoice>(e =>
            {
                e.ToTable("invoice", "eaship");
                e.HasKey(x => x.InvoiceId);

                e.Property(x => x.InvoiceId).HasColumnName("invoice_id");
                e.Property(x => x.ContractId).HasColumnName("contract_id");
                e.Property(x => x.Number).HasColumnName("number");
                e.Property(x => x.Amount).HasColumnName("amount").HasColumnType("numeric(18,2)");
                e.Property(x => x.IssuedAt).HasColumnName("issued_at");
                e.Property(x => x.DueDate).HasColumnName("due_date");
                e.Property(x => x.PaidAt).HasColumnName("paid_at");
                e.Property(x => x.PdfUrl).HasColumnName("pdf_url");
                e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();
            });
        }
    }
}
