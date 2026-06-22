using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDireccionObjetoInterventorCenso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "CENSO",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "objeto_interventor",
                table: "CENSO",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "direccion",
                table: "CENSO");

            migrationBuilder.DropColumn(
                name: "objeto_interventor",
                table: "CENSO");
        }
    }
}
