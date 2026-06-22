using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposExtendidosCenso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "area",
                table: "CENSO",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "geolocalizacion",
                table: "CENSO",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "microterritorio",
                table: "CENSO",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "territorio",
                table: "CENSO",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "total_miembros",
                table: "CENSO",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "visitantes_temporales_col",
                table: "CENSO",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "visitantes_temporales_mig",
                table: "CENSO",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "geolocalizacion",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "microterritorio",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "territorio",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "total_miembros",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "visitantes_temporales_col",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "visitantes_temporales_mig",
                table: "CENSO");
        }
    }
}
