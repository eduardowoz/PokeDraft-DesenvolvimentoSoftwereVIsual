using System.ComponentModel.DataAnnotations;
using PokeDraft.Web.Models.Enums;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Um Pokémon do catálogo, importado da PokéAPI. É a "carta" que os jogadores
/// podem banir ou comprar durante o draft.
/// </summary>
public class Pokemon
{
    public int Id { get; set; }

    /// <summary>Id do Pokémon na PokéAPI (usado pelo importador para não duplicar).</summary>
    public int PokeApiId { get; set; }

    [MaxLength(60)]
    public string Nome { get; set; } = null!;

    public TipoPokemon Tipo1 { get; set; }

    /// <summary>Segundo tipo, quando o Pokémon tem tipagem dupla.</summary>
    public TipoPokemon? Tipo2 { get; set; }

    // Status base (vindos da PokéAPI) — entram no cálculo do overall.
    public int HpBase { get; set; }
    public int AtaqueBase { get; set; }
    public int DefesaBase { get; set; }
    public int AtaqueEspecialBase { get; set; }
    public int DefesaEspecialBase { get; set; }
    public int VelocidadeBase { get; set; }

    /// <summary>Soma dos seis status base — atalho para ranquear e precificar o Pokémon.</summary>
    public int TotalStatusBase { get; set; }

    public int Geracao { get; set; }

    [MaxLength(300)]
    public string? SpriteUrl { get; set; }

    /// <summary>Custo em pontos para adicionar este Pokémon ao time.</summary>
    public int Custo { get; set; }

    // Relacionamentos
    public ICollection<PokemonNoPool> AparicoesEmPools { get; set; } = new List<PokemonNoPool>();
    public ICollection<EscolhaPokemon> Escolhas { get; set; } = new List<EscolhaPokemon>();
}
