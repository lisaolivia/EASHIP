using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Eaship.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "eaship");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:eaship.user_role", "renter,admin")
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "booking",
                schema: "eaship",
                columns: table => new
                {
                    booking_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    origin_port = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    destination_port = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    cargo_desc = table.Column<string>(type: "text", nullable: false),
                    harga_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.booking_id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                schema: "eaship",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    npwp = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    contact = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "contract",
                schema: "eaship",
                columns: table => new
                {
                    contract_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    pdf_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    renter_signed = table.Column<bool>(type: "boolean", nullable: false),
                    renter_signer = table.Column<string>(type: "text", nullable: true),
                    renter_signed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    owner_signed = table.Column<bool>(type: "boolean", nullable: false),
                    owner_signer = table.Column<string>(type: "text", nullable: true),
                    owner_signed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract", x => x.contract_id);
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                schema: "eaship",
                columns: table => new
                {
                    invoice_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contract_id = table.Column<long>(type: "bigint", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    issued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pdf_url = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice", x => x.invoice_id);
                });

            migrationBuilder.CreateTable(
                name: "renter_company",
                schema: "eaship",
                columns: table => new
                {
                    renter_company_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nama = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    npwp = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    CityProvince = table.Column<string>(type: "text", nullable: false),
                    EmailBilling = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PICName = table.Column<string>(type: "text", nullable: false),
                    PICPosition = table.Column<string>(type: "text", nullable: false),
                    PICEmail = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renter_company", x => x.renter_company_id);
                });

            migrationBuilder.CreateTable(
                name: "tugboat",
                schema: "eaship",
                columns: table => new
                {
                    tugboat_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nama = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tugboat_hp = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tugboat", x => x.tugboat_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "eaship",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    renter_company_id = table.Column<int>(type: "integer", nullable: true),
                    email = table.Column<string>(type: "citext", nullable: false),
                    full_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    password_salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tongkang",
                schema: "eaship",
                columns: table => new
                {
                    tongkang_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    kapasitas_dwt = table.Column<string>(type: "text", nullable: false),
                    include_tugboat = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    CompanyId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tongkang", x => x.tongkang_id);
                    table.ForeignKey(
                        name: "FK_tongkang_company_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "eaship",
                        principalTable: "company",
                        principalColumn: "company_id");
                });

            migrationBuilder.CreateTable(
                name: "booking_tongkang",
                schema: "eaship",
                columns: table => new
                {
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    tongkang_id = table.Column<long>(type: "bigint", nullable: false),
                    DaysAllocated = table.Column<int>(type: "integer", nullable: true),
                    SequenceNo = table.Column<int>(type: "integer", nullable: true),
                    BookingId1 = table.Column<long>(type: "bigint", nullable: true),
                    TongkangId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_tongkang", x => new { x.booking_id, x.tongkang_id });
                    table.ForeignKey(
                        name: "FK_booking_tongkang_booking_BookingId1",
                        column: x => x.BookingId1,
                        principalSchema: "eaship",
                        principalTable: "booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_booking_tongkang_booking_booking_id",
                        column: x => x.booking_id,
                        principalSchema: "eaship",
                        principalTable: "booking",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_tongkang_tongkang_TongkangId1",
                        column: x => x.TongkangId1,
                        principalSchema: "eaship",
                        principalTable: "tongkang",
                        principalColumn: "tongkang_id");
                    table.ForeignKey(
                        name: "FK_booking_tongkang_tongkang_tongkang_id",
                        column: x => x.tongkang_id,
                        principalSchema: "eaship",
                        principalTable: "tongkang",
                        principalColumn: "tongkang_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tongkang_tugboat",
                schema: "eaship",
                columns: table => new
                {
                    tongkang_id = table.Column<long>(type: "bigint", nullable: false),
                    tugboat_id = table.Column<long>(type: "bigint", nullable: false),
                    TongkangId1 = table.Column<long>(type: "bigint", nullable: true),
                    TugboatId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tongkang_tugboat", x => new { x.tongkang_id, x.tugboat_id });
                    table.ForeignKey(
                        name: "FK_tongkang_tugboat_tongkang_TongkangId1",
                        column: x => x.TongkangId1,
                        principalSchema: "eaship",
                        principalTable: "tongkang",
                        principalColumn: "tongkang_id");
                    table.ForeignKey(
                        name: "FK_tongkang_tugboat_tongkang_tongkang_id",
                        column: x => x.tongkang_id,
                        principalSchema: "eaship",
                        principalTable: "tongkang",
                        principalColumn: "tongkang_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tongkang_tugboat_tugboat_TugboatId1",
                        column: x => x.TugboatId1,
                        principalSchema: "eaship",
                        principalTable: "tugboat",
                        principalColumn: "tugboat_id");
                    table.ForeignKey(
                        name: "FK_tongkang_tugboat_tugboat_tugboat_id",
                        column: x => x.tugboat_id,
                        principalSchema: "eaship",
                        principalTable: "tugboat",
                        principalColumn: "tugboat_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_tongkang_BookingId1",
                schema: "eaship",
                table: "booking_tongkang",
                column: "BookingId1");

            migrationBuilder.CreateIndex(
                name: "IX_booking_tongkang_tongkang_id",
                schema: "eaship",
                table: "booking_tongkang",
                column: "tongkang_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_tongkang_TongkangId1",
                schema: "eaship",
                table: "booking_tongkang",
                column: "TongkangId1");

            migrationBuilder.CreateIndex(
                name: "IX_tongkang_CompanyId1",
                schema: "eaship",
                table: "tongkang",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_tongkang_tugboat_TongkangId1",
                schema: "eaship",
                table: "tongkang_tugboat",
                column: "TongkangId1");

            migrationBuilder.CreateIndex(
                name: "IX_tongkang_tugboat_tugboat_id",
                schema: "eaship",
                table: "tongkang_tugboat",
                column: "tugboat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tongkang_tugboat_TugboatId1",
                schema: "eaship",
                table: "tongkang_tugboat",
                column: "TugboatId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking_tongkang",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "contract",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "invoice",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "renter_company",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "tongkang_tugboat",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "users",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "booking",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "tongkang",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "tugboat",
                schema: "eaship");

            migrationBuilder.DropTable(
                name: "company",
                schema: "eaship");
        }
    }
}
