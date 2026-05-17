using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmekSepetiWeb.Migrations
{
    /// <inheritdoc />
    public partial class SepetTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SepetElemanlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UygulamaKullanicisiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepetElemanlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SepetElemanlari_AspNetUsers_UygulamaKullanicisiId",
                        column: x => x.UygulamaKullanicisiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SepetElemanlari_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SepetElemanlari_UrunId",
                table: "SepetElemanlari",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetElemanlari_UygulamaKullanicisiId",
                table: "SepetElemanlari",
                column: "UygulamaKullanicisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SepetElemanlari");
        }
    }
}
