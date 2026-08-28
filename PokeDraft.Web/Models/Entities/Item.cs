using System.ComponentModel.DataAnnotations;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um item de batalha do catálogo. Pode ser equipado em um Pokémon do time,
/// somando ao overall dele conforme sua utilidade.
/// </summary>
public class Item
{
    public int Id { get; set; }

    [MaxLength(80)]
    public string Nome { get; set; } = null!;

    [MaxLength(400)]
    public string? Descricao { get; set; }

    /// <summary>Categoria livre do item (ex.: "Berry", "Held Item", "Choice").</summary>
    [MaxLength(60)]
    public string? Categoria { get; set; }

    /// <summary>Bônus fixo que o item adiciona ao overall do Pokémon que o carrega.</summary>
    public int BonusOverall { get; set; }

    [MaxLength(300)]
    public string? SpriteUrl { get; set; }

    /// <summary>Custo em pontos para adquirir este item no draft.</summary>
    public int Custo { get; set; }

    // Relacionamentos
    public ICollection<ItemNoPool> AparicoesEmPools { get; set; } = new List<ItemNoPool>();
    public ICollection<EscolhaPokemon> EscolhasEquipadas { get; set; } = new List<EscolhaPokemon>();
}
