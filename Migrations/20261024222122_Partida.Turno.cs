using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedraft.Migrations
{
    /// <inheritdoc />
    public partial class PartidaTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores");

            migrationBuilder.AlterColumn<int>(
                name: "JogadorId",
                table: "PokemonsJogadores",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Turno",
                table: "Partidas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores");

            migrationBuilder.DropColumn(
                name: "Turno",
                table: "Partidas");

            migrationBuilder.AlterColumn<int>(
                name: "JogadorId",
                table: "PokemonsJogadores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
