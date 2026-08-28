namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um duelo 1x1 dentro da batalha simulada: o Pokémon na posição N do jogador A
/// contra o Pokémon na posição N do jogador B, com os overalls e o vencedor.
/// </summary>
public class ConfrontoBatalha
{
    public int Id { get; set; }

    public int PartidaId { get; set; }
    public Partida Partida { get; set; } = null!;

    /// <summary>Posição do confronto (1 = primeiros Pokémon de cada time).</summary>
    public int Ordem { get; set; }

    public int EscolhaPokemonAId { get; set; }
    public EscolhaPokemon EscolhaPokemonA { get; set; } = null!;

    public int EscolhaPokemonBId { get; set; }
    public EscolhaPokemon EscolhaPokemonB { get; set; } = null!;

    /// <summary>Overall final do lado A no confronto (base + extras de batalha).</summary>
    public int OverallA { get; set; }

    /// <summary>Overall final do lado B no confronto (base + extras de batalha).</summary>
    public int OverallB { get; set; }

    /// <summary>Jogador que venceu este confronto (nulo em caso de empate no confronto).</summary>
    public int? VencedorJogadorId { get; set; }
}
