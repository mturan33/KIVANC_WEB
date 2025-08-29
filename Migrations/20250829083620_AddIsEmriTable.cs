using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KIVANC_WEB.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEmriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IsEmirleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Konu = table.Column<string>(type: "TEXT", nullable: false),
                    AtananKisi = table.Column<string>(type: "TEXT", nullable: false),
                    Lokasyon = table.Column<string>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TamamlandiMi = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsEmirleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IsEmirleri");
        }
    }
}
