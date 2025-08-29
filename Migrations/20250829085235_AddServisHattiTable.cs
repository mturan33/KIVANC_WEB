using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KIVANC_WEB.Migrations
{
    /// <inheritdoc />
    public partial class AddServisHattiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServisHatları",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HatAdi = table.Column<string>(type: "TEXT", nullable: false),
                    Guzergah = table.Column<string>(type: "TEXT", nullable: false),
                    SoforAdi = table.Column<string>(type: "TEXT", nullable: false),
                    Plaka = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisHatları", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServisHatları");
        }
    }
}
