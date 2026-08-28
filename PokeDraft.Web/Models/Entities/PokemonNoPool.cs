namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um Pokémon sorteado para um sample (pacote) do draft de uma partida.
/// Os dois jogadores enxergam os mesmos pools; daqui saem os banimentos e as compras.
/// </summary>
public class PokemonNoPool
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;

    public int PokemonId { get; set; }
    public Pokemon Pokemon { get; set; } = null!;

    /// <summary>Número do sample (1..N) em que este Pokémon apareceu.</summary>
    public int NumeroSample { get; set; }

    /// <summary>True se algum jogador baniu este Pokémon — ninguém mais pode comprá-lo.</summary>
    public bool Banido { get; set; }

    /// <summary>Jogador que baniu (quando <see cref="Banido"/> é true).</summary>
    public int? BanidoPorJogadorId { get; set; }

    /// <summary>Jogador que comprou este Pokémon do pool (nulo enquanto está disponível).</summary>
    public int? CompradoPorJogadorId { get; set; }
}
