using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmekSepetiWeb.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciIdNullableYaptim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_AspNetUsers_UygulamaKullanicisiId",
                table: "Urunler");

            migrationBuilder.AlterColumn<string>(
                name: "UygulamaKullanicisiId",
                table: "Urunler",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_AspNetUsers_UygulamaKullanicisiId",
                table: "Urunler",
                column: "UygulamaKullanicisiId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_AspNetUsers_UygulamaKullanicisiId",
                table: "Urunler");

            migrationBuilder.AlterColumn<string>(
                name: "UygulamaKullanicisiId",
                table: "Urunler",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_AspNetUsers_UygulamaKullanicisiId",
                table: "Urunler",
                column: "UygulamaKullanicisiId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
