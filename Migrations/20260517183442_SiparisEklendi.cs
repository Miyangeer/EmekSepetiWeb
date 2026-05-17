using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmekSepetiWeb.Migrations
{
    /// <inheritdoc />
    public partial class SiparisEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Siparisler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UygulamaKullanicisiId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToplamTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TeslimatTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeslimatAdresi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siparisler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Siparisler");
        }
    }
}
