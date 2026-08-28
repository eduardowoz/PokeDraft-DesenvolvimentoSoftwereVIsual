using System.ComponentModel.DataAnnotations;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Conjunto de regras reutilizável para uma partida (o "preset" do draft).
/// Uma partida aponta para uma configuração.
/// </summary>
public class Configuracao
{
    public int Id { get; set; }

    [MaxLength(80)]
    public string Nome { get; set; } = null!;

    /// <summary>Pontos que cada jogador recebe para montar o time.</summary>
    public int BudgetPontos { get; set; } = 100;

    /// <summary>Quantos samples (pacotes) de Pokémon são sorteados no draft.</summary>
    public int QuantidadeSamples { get; set; } = 5;

    /// <summary>Quantos Pokémon cada sample contém.</summary>
    public int PokemonPorSample { get; set; } = 6;

    /// <summary>Tamanho máximo do time de cada jogador.</summary>
    public int TamanhoTime { get; set; } = 6;

    /// <summary>Quantos banimentos cada jogador pode fazer.</summary>
    public int BansPorJogador { get; set; } = 2;

    /// <summary>
    /// Fator aplicado ao custo na hora da venda (ex.: 0.5 = jogador recupera metade dos pontos).
    /// </summary>
    public double FatorVenda { get; set; } = 0.5;

    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
}
