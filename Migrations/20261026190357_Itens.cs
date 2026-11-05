using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedraft.Migrations
{
    /// <inheritdoc />
    public partial class Itens : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "PokemonsJogadores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Custo = table.Column<int>(type: "INTEGER", nullable: false),
                    Overall = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonsJogadores_ItemId",
                table: "PokemonsJogadores",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonsJogadores_Itens_ItemId",
                table: "PokemonsJogadores",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonsJogadores_Itens_ItemId",
                table: "PokemonsJogadores");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropIndex(
                name: "IX_PokemonsJogadores_ItemId",
                table: "PokemonsJogadores");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "PokemonsJogadores");

            migrationBuilder.AlterColumn<int>(
                name: "JogadorId",
                table: "PokemonsJogadores",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonsJogadores_Jogadores_JogadorId",
                table: "PokemonsJogadores",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id");
        }
    }
}
