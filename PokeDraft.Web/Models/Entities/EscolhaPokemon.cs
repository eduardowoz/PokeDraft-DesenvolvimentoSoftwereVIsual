using System.ComponentModel.DataAnnotations;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um Pokémon que um jogador comprou e colocou no time durante o draft,
/// com o item equipado, a natureza/IVs sorteados e a posição na batalha.
/// </summary>
public class EscolhaPokemon
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;

    public int JogadorId { get; set; }
    public Jogador Jogador { get; set; } = null!;

    public int PokemonId { get; set; }
    public Pokemon Pokemon { get; set; } = null!;

    /// <summary>Item equipado neste Pokémon (opcional).</summary>
    public int? ItemId { get; set; }
    public Item? Item { get; set; }

    /// <summary>Pontos gastos para comprar este Pokémon (custo no momento da compra).</summary>
    public int PontosGastos { get; set; }

    /// <summary>Ordem do Pokémon na batalha (1 = enfrenta o 1º do oponente).</summary>
    public int? OrdemNaBatalha { get; set; }

    // Natureza e IVs são sorteados (fora do escopo escolher manualmente),
    // mas entram no cálculo do overall.
    [MaxLength(20)]
    public string? Natureza { get; set; }

    public int IvHp { get; set; }
    public int IvAtaque { get; set; }
    public int IvDefesa { get; set; }
    public int IvAtaqueEspecial { get; set; }
    public int IvDefesaEspecial { get; set; }
    public int IvVelocidade { get; set; }

    /// <summary>Overall calculado fora da batalha (status + item + natureza + IV).</summary>
    public int OverallBase { get; set; }

    // Venda: jogador pode vender um Pokémon do time por pontos reduzidos.
    public bool Vendida { get; set; }
    public int? PontosRecebidosNaVenda { get; set; }
}
