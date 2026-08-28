using System.ComponentModel.DataAnnotations;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Uma pessoa que participa de partidas. Duas por partida.
/// </summary>
public class Jogador
{
    public int Id { get; set; }

    [MaxLength(60)]
    public string Nome { get; set; } = null!;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ICollection<EscolhaPokemon> Escolhas { get; set; } = new List<EscolhaPokemon>();
    public ICollection<MovimentacaoDraft> Movimentacoes { get; set; } = new List<MovimentacaoDraft>();
}
