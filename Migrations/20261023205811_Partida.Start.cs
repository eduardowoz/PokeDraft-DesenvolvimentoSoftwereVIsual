using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedraft.Migrations
{
    /// <inheritdoc />
    public partial class PartidaStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Start",
                table: "Partidas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Start",
                table: "Partidas");
        }
    }
}
