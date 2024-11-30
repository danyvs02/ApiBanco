using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBanco.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicialModelosContrasena : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contrasena",
                table: "usuarios",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contrasena",
                table: "usuarios");
        }
    }
}
