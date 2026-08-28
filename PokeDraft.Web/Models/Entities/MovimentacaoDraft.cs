using System.ComponentModel.DataAnnotations;
using PokeDraft.Web.Models.Enums;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Registro de uma ação feita por um jogador no draft (ban, compra, venda, equipar item).
/// Serve de histórico/auditoria e permite reconstruir o saldo de pontos.
/// </summary>
public class MovimentacaoDraft
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;

    public int JogadorId { get; set; }
    public Jogador Jogador { get; set; } = null!;

    public TipoMovimentacao Tipo { get; set; }

    /// <summary>Pokémon envolvido (quando aplicável).</summary>
    public int? PokemonId { get; set; }
    public Pokemon? Pokemon { get; set; }

    /// <summary>Item envolvido (quando aplicável).</summary>
    public int? ItemId { get; set; }
    public Item? Item { get; set; }

    /// <summary>
    /// Variação de pontos causada pela ação: negativa em compras, positiva em vendas, zero em bans.
    /// </summary>
    public int VariacaoPontos { get; set; }

    /// <summary>Saldo de pontos do jogador logo após esta movimentação.</summary>
    public int SaldoResultante { get; set; }

    [MaxLength(200)]
    public string? Descricao { get; set; }

    public DateTime Momento { get; set; } = DateTime.UtcNow;
}
