using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmekSepetiWeb.Migrations
{
    /// <inheritdoc />
    public partial class SeedVerileriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Kategoriler",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Ev Yemekleri & Gıda" },
                    { 2, "Takı & Aksesuar" },
                    { 3, "Ahşap & Oyuncak" }
                });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "Aciklama", "Ad", "Fiyat", "KategoriId", "OlusturmaTarihi", "ResimUrl", "UygulamaKullanicisiId" },
                values: new object[,]
                {
                    { 1, "Ev yapımı nefis Kayseri mantısı", "Kayseri Usulü Mantı", 350.00m, 1, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "manti.jpg", null },
                    { 2, "Özel tasarım doğal taşlı şık kolye", "Doğal Taşlı Kolye", 190.00m, 2, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "kolye.jpg", null },
                    { 3, "Çocuklar için sağlıklı ahşap oyuncak tren", "Ahşap Oyuncak Tren", 280.00m, 3, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "tren.jpg", null },
                    { 8, "Gri ve günlük kullanıma uygun el örgüsü atkı", "El Örgüsü Atkı", 149.99m, 2, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "el örgüsü atkı.png", null },
                    { 9, "Ürün tahtadan yapılmıştır el emeğidir", "Tahta Pinokyo", 500.00m, 3, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "tahta pinokyo.png", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Urunler",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Kategoriler",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
