using Eaship.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaship.Models
{
    public class EashipDbContext : DbContext
    {
        public EashipDbContext(DbContextOptions<EashipDbContext> options) : base(options) { }

        // Daftar tabel (DbSet)
        public DbSet<User> Users => Set<User>();
        public DbSet<RenterCompany> RenterCompanies => Set<RenterCompany>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Tongkang> Tongkangs => Set<Tongkang>();
        public DbSet<Tugboat> Tugboats => Set<Tugboat>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        // Tabel penghubung (many-to-many)
        public DbSet<TongkangTugboat> TongkangTugboats => Set<TongkangTugboat>();
        public DbSet<BookingTongkang> BookingTongkangs => Set<BookingTongkang>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gunakan schema "eaship"
            modelBuilder.HasDefaultSchema("eaship");

            // USER

            modelBuilder.HasPostgresEnum<UserRole>("eaship", "user_role");
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users","eaship");
                e.HasKey(x => x.UserId);

                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.RenterCompanyId).HasColumnName("renter_company_id");

                e.Property(x => x.Email).HasColumnName("email").HasColumnType("citext");
                e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(250);

                e.Property(x => x.PasswordHash).HasColumnName("password_hash");
                e.Property(x => x.PasswordSalt).HasColumnName("password_salt");

                e.Property(x => x.Role).HasColumnName("role").HasConversion<string>();
                e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");

                // (Opsional) FK ke renter_company
                // e.HasOne<RenterCompany>().WithMany().HasForeignKey(x => x.RenterCompanyId);
            });


            // COMPANY
            modelBuilder.Entity<Company>(e =>
            {
                e.ToTable("company", "eaship");
                e.HasKey(x => x.CompanyId);
                e.Property(x => x.CompanyId).HasColumnName("company_id");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(250);
                e.Property(x => x.NPWP).HasColumnName("npwp").HasMaxLength(250);
                e.Property(x => x.Contact).HasColumnName("contact").HasMaxLength(250);
                e.Property(x => x.Address).HasColumnName("address"); // text
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            // TUGBOAT
           // modelBuilder.HasPostgresEnum<TugboatStatus>("eaship", "asset_status");
            modelBuilder.Entity<Tugboat>(e =>
            {
                e.ToTable("tugboat", "eaship");
                e.HasKey(x => x.TugboatId);

                e.Property(x => x.TugboatId).HasColumnName("tugboat_id");
                e.Property(x => x.Nama).HasColumnName("nama").HasMaxLength(250);
                e.Property(x => x.TugboatHp).HasColumnName("tugboat_hp").HasMaxLength(250);

                e.Property(x => x.Status)
                    .HasColumnName("status")
                    .HasConversion<string>(); // atau .HasColumnType("eaship.asset_status");

                e.Property(x => x.CompanyId).HasColumnName("company_id");
            });


            // TONGKANG
            modelBuilder.Entity<Tongkang>(e =>
            {
                e.ToTable("tongkang", "eaship");
                e.HasKey(x => x.TongkangId);

                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200);

                // Kapasitas angka → jangan pakai HasMaxLength
                e.Property(x => x.KapasitasDwt).HasColumnName("kapasitas_dwt");

                e.Property(x => x.CompanyId).HasColumnName("company_id");
                e.Property(x => x.IncludeTugboat).HasColumnName("include_tugboat");

                e.Property(x => x.Status)
                    .HasColumnName("status")
                    .HasConversion<string>(); // atau PG enum kalau ada
            });


            // TONGKANG_TUGBOAT
            modelBuilder.Entity<TongkangTugboat>(e =>
            {
                e.ToTable("tongkang_tugboat", "eaship");
                e.HasKey(x => new { x.TongkangId, x.TugboatId });

                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");
                e.Property(x => x.TugboatId).HasColumnName("tugboat_id"); // pastikan tipe long di model!

                e.HasOne<Tongkang>()
                    .WithMany()
                    .HasForeignKey(x => x.TongkangId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<Tugboat>()
                    .WithMany()
                    .HasForeignKey(x => x.TugboatId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RENTER COMPANY
            modelBuilder.Entity<RenterCompany>(e =>
            {
                e.ToTable("renter_company", "eaship");
                e.HasKey(x => x.RenterCompanyId);
                e.Property(x => x.RenterCompanyId).HasColumnName("renter_company_id");
                e.Property(x => x.Nama).HasColumnName("nama").HasMaxLength(200);
                e.Property(x => x.NPWP).HasColumnName("npwp").HasMaxLength(250);
                e.Property(x => x.Contact).HasColumnName("contact").HasMaxLength(250);
                e.Property(x => x.Address).HasColumnName("address").HasColumnType("text");
                e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();
            });

            // BOOKING
            modelBuilder.Entity<Booking>(e =>
            {
                e.ToTable("booking", "eaship");

                e.HasKey(b => b.BookingId);
                e.Property(b => b.BookingId)
                      .HasColumnName("booking_id");

                e.Property(b => b.UserId)
                      .HasColumnName("user_id");

                e.Property(b => b.OriginPort)
                      .HasColumnName("origin_port")
                      .HasMaxLength(250);

                e.Property(b => b.DestinationPort)
                      .HasColumnName("destination_port")
                      .HasMaxLength(250);

                e.Property(b => b.StartDate)
                      .HasColumnName("start_date");

                e.Property(b => b.DurationDays)
                      .HasColumnName("duration_days");

                e.Property(b => b.CargoDesc)
                      .HasColumnName("cargo_desc");

                e.Property(b => b.HargaTotal)
                      .HasColumnName("harga_total")
                      .HasColumnType("numeric(18,2)");

                // enum mapping: simpan sebagai string atau ke tipe enum PostgreSQL
                e.Property(b => b.Status)
                      .HasColumnName("status")
                      .HasConversion<string>(); // atau .HasColumnType("eaship.booking_status");

                e.Property(b => b.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamp with time zone");
            });

            // BOOKING_TONGKANG
            modelBuilder.Entity<BookingTongkang>(e =>
            {
                e.ToTable("booking_tongkang", "eaship");
                e.HasKey(x => new { x.BookingId, x.TongkangId });

                e.Property(x => x.BookingId).HasColumnName("booking_id");
                e.Property(x => x.TongkangId).HasColumnName("tongkang_id");

                // (Opsional) metadata
                // e.Property(x => x.DaysAllocated).HasColumnName("days_allocated");
                // e.Property(x => x.SequenceNo).HasColumnName("sequence_no");

                e.HasOne<Booking>()
                    .WithMany(b => b.BookingTongkangs)
                    .HasForeignKey(x => x.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<Tongkang>()
                    .WithMany() // atau WithMany(t => t.BookingTongkangs) kalau ada navigasi di Tongkang
                    .HasForeignKey(x => x.TongkangId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // KONTRAK
            modelBuilder.Entity<Contract>(e =>
            {
                e.ToTable("contract", "eaship");
                e.HasKey(x => x.ContractId);

                e.Property(x => x.ContractId).HasColumnName("contract_id");
                e.Property(x => x.BookingId).HasColumnName("booking_id"); 

                e.Property(x => x.PdfUrl).HasColumnName("pdf_url").HasMaxLength(500);

                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

                e.Property(x => x.RenterSigned).HasColumnName("renter_signed");
                e.Property(x => x.RenterSigner).HasColumnName("renter_signer");
                e.Property(x => x.RenterSignedAt).HasColumnName("renter_signed_at");

                e.Property(x => x.OwnerSigned).HasColumnName("owner_signed");
                e.Property(x => x.OwnerSigner).HasColumnName("owner_signer");
                e.Property(x => x.OwnerSignedAt).HasColumnName("owner_signed_at");

                e.Property(x => x.ApprovedAt).HasColumnName("approved_at");
            });


            // INVOICE
            modelBuilder.Entity<Invoice>(e =>
            {
                e.ToTable("invoice", "eaship");
                e.HasKey(x => x.InvoiceId);

                e.Property(x => x.InvoiceId).HasColumnName("invoice_id");
                e.Property(x => x.ContractId).HasColumnName("contract_id"); // FIX

                e.Property(x => x.Number).HasColumnName("number");
                e.Property(x => x.Amount).HasColumnName("amount").HasColumnType("numeric(18,2)");

                e.Property(x => x.IssuedAt).HasColumnName("issued_at"); // FIX selector
                e.Property(x => x.DueDate).HasColumnName("due_date");
                e.Property(x => x.PaidAt).HasColumnName("paid_at"); // FIX

                e.Property(x => x.PdfUrl).HasColumnName("pdf_url");
                e.Property(x => x.Status).HasColumnName("status").HasConversion<string>();
            });
        }
    }
}