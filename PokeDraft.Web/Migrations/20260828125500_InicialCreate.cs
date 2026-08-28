using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeDraft.Web.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    BudgetPontos = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeSamples = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonPorSample = table.Column<int>(type: "INTEGER", nullable: false),
                    TamanhoTime = table.Column<int>(type: "INTEGER", nullable: false),
                    BansPorJogador = table.Column<int>(type: "INTEGER", nullable: false),
                    FatorVenda = table.Column<double>(type: "REAL", nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    Categoria = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    BonusOverall = table.Column<int>(type: "INTEGER", nullable: false),
                    SpriteUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Custo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jogadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PokeApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Tipo1 = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Tipo2 = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    HpBase = table.Column<int>(type: "INTEGER", nullable: false),
                    AtaqueBase = table.Column<int>(type: "INTEGER", nullable: false),
                    DefesaBase = table.Column<int>(type: "INTEGER", nullable: false),
                    AtaqueEspecialBase = table.Column<int>(type: "INTEGER", nullable: false),
                    DefesaEspecialBase = table.Column<int>(type: "INTEGER", nullable: false),
                    VelocidadeBase = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalStatusBase = table.Column<int>(type: "INTEGER", nullable: false),
                    Geracao = table.Column<int>(type: "INTEGER", nullable: false),
                    SpriteUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Custo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinalizadaEm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConfiguracaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    JogadorAId = table.Column<int>(type: "INTEGER", nullable: false),
                    JogadorBId = table.Column<int>(type: "INTEGER", nullable: false),
                    VencedorJogadorId = table.Column<int>(type: "INTEGER", nullable: true),
                    PontosJogadorA = table.Column<int>(type: "INTEGER", nullable: false),
                    PontosJogadorB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partidas_Configuracoes_ConfiguracaoId",
                        column: x => x.ConfiguracaoId,
                        principalTable: "Configuracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jogadores_JogadorAId",
                        column: x => x.JogadorAId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jogadores_JogadorBId",
                        column: x => x.JogadorBId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jogadores_VencedorJogadorId",
                        column: x => x.VencedorJogadorId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EscolhasPokemon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    JogadorId = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    PontosGastos = table.Column<int>(type: "INTEGER", nullable: false),
                    OrdemNaBatalha = table.Column<int>(type: "INTEGER", nullable: true),
                    Natureza = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    IvHp = table.Column<int>(type: "INTEGER", nullable: false),
                    IvAtaque = table.Column<int>(type: "INTEGER", nullable: false),
                    IvDefesa = table.Column<int>(type: "INTEGER", nullable: false),
                    IvAtaqueEspecial = table.Column<int>(type: "INTEGER", nullable: false),
                    IvDefesaEspecial = table.Column<int>(type: "INTEGER", nullable: false),
                    IvVelocidade = table.Column<int>(type: "INTEGER", nullable: false),
                    OverallBase = table.Column<int>(type: "INTEGER", nullable: false),
                    Vendida = table.Column<bool>(type: "INTEGER", nullable: false),
                    PontosRecebidosNaVenda = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscolhasPokemon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscolhasPokemon_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EscolhasPokemon_Jogadores_JogadorId",
                        column: x => x.JogadorId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EscolhasPokemon_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EscolhasPokemon_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensNoPool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdquiridoPorJogadorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensNoPool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensNoPool_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensNoPool_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesDraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    JogadorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    VariacaoPontos = table.Column<int>(type: "INTEGER", nullable: false),
                    SaldoResultante = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Momento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesDraft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacoesDraft_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesDraft_Jogadores_JogadorId",
                        column: x => x.JogadorId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesDraft_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimentacoesDraft_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PokemonNoPool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroSample = table.Column<int>(type: "INTEGER", nullable: false),
                    Banido = table.Column<bool>(type: "INTEGER", nullable: false),
                    BanidoPorJogadorId = table.Column<int>(type: "INTEGER", nullable: true),
                    CompradoPorJogadorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonNoPool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PokemonNoPool_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonNoPool_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfrontosBatalha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartidaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    EscolhaPokemonAId = table.Column<int>(type: "INTEGER", nullable: false),
                    EscolhaPokemonBId = table.Column<int>(type: "INTEGER", nullable: false),
                    OverallA = table.Column<int>(type: "INTEGER", nullable: false),
                    OverallB = table.Column<int>(type: "INTEGER", nullable: false),
                    VencedorJogadorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfrontosBatalha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfrontosBatalha_EscolhasPokemon_EscolhaPokemonAId",
                        column: x => x.EscolhaPokemonAId,
                        principalTable: "EscolhasPokemon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfrontosBatalha_EscolhasPokemon_EscolhaPokemonBId",
                        column: x => x.EscolhaPokemonBId,
                        principalTable: "EscolhasPokemon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfrontosBatalha_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Configuracoes",
                columns: new[] { "Id", "BansPorJogador", "BudgetPontos", "CriadaEm", "FatorVenda", "Nome", "PokemonPorSample", "QuantidadeSamples", "TamanhoTime" },
                values: new object[] { 1, 2, 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.5, "Padrão", 6, 5, 6 });

            migrationBuilder.CreateIndex(
                name: "IX_Configuracoes_Nome",
                table: "Configuracoes",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontosBatalha_EscolhaPokemonAId",
                table: "ConfrontosBatalha",
                column: "EscolhaPokemonAId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontosBatalha_EscolhaPokemonBId",
                table: "ConfrontosBatalha",
                column: "EscolhaPokemonBId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontosBatalha_PartidaId",
                table: "ConfrontosBatalha",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_EscolhasPokemon_ItemId",
                table: "EscolhasPokemon",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EscolhasPokemon_JogadorId",
                table: "EscolhasPokemon",
                column: "JogadorId");

            migrationBuilder.CreateIndex(
                name: "IX_EscolhasPokemon_PartidaId",
                table: "EscolhasPokemon",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_EscolhasPokemon_PokemonId",
                table: "EscolhasPokemon",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensNoPool_ItemId",
                table: "ItensNoPool",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensNoPool_PartidaId",
                table: "ItensNoPool",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDraft_ItemId",
                table: "MovimentacoesDraft",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDraft_JogadorId",
                table: "MovimentacoesDraft",
                column: "JogadorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDraft_PartidaId",
                table: "MovimentacoesDraft",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDraft_PokemonId",
                table: "MovimentacoesDraft",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_ConfiguracaoId",
                table: "Partidas",
                column: "ConfiguracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_JogadorAId",
                table: "Partidas",
                column: "JogadorAId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_JogadorBId",
                table: "Partidas",
                column: "JogadorBId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_VencedorJogadorId",
                table: "Partidas",
                column: "VencedorJogadorId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonNoPool_PartidaId_NumeroSample",
                table: "PokemonNoPool",
                columns: new[] { "PartidaId", "NumeroSample" });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonNoPool_PokemonId",
                table: "PokemonNoPool",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_PokeApiId",
                table: "Pokemons",
                column: "PokeApiId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfrontosBatalha");

            migrationBuilder.DropTable(
                name: "ItensNoPool");

            migrationBuilder.DropTable(
                name: "MovimentacoesDraft");

            migrationBuilder.DropTable(
                name: "PokemonNoPool");

            migrationBuilder.DropTable(
                name: "EscolhasPokemon");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropTable(
                name: "Jogadores");
        }
    }
}
