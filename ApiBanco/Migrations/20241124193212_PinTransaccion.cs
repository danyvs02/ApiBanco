using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBanco.Migrations
{
    /// <inheritdoc />
    public partial class PinTransaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "clave",
                table: "transacciones",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "clave",
                table: "transacciones");
        }
    }
}
