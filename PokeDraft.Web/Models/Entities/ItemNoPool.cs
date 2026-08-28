namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um item disponível no sample único de itens de uma partida.
/// Compartilhado pelos dois jogadores.
/// </summary>
public class ItemNoPool
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;

    /// <summary>Jogador que pegou este item (nulo enquanto está disponível).</summary>
    public int? AdquiridoPorJogadorId { get; set; }
}
