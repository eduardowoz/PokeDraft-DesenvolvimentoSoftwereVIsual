using System.ComponentModel.DataAnnotations;
using PokeDraft.Web.Models.Enums;

namespace PokeDraft.Web.Models.Entities;

/// <summary>
/// Uma disputa entre dois jogadores: draft dos times e simulação da batalha.
/// É a entidade central que amarra pools, escolhas, movimentações e confrontos.
/// </summary>
public class Partida
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Nome { get; set; }

    public StatusPartida Status { get; set; } = StatusPartida.Configuracao;

    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    public DateTime? FinalizadaEm { get; set; }

    // Configuração (regras) usada nesta partida
    public int ConfiguracaoId { get; set; }
    public Configuracao Configuracao { get; set; } = null!;

    // Os dois jogadores
    public int JogadorAId { get; set; }
    public Jogador JogadorA { get; set; } = null!;

    public int JogadorBId { get; set; }
    public Jogador JogadorB { get; set; } = null!;

    /// <summary>Vencedor da batalha simulada (nulo enquanto a partida não termina).</summary>
    public int? VencedorJogadorId { get; set; }
    public Jogador? VencedorJogador { get; set; }

    // Placar final da batalha (nº de confrontos vencidos por cada lado)
    public int PontosJogadorA { get; set; }
    public int PontosJogadorB { get; set; }

    // Relacionamentos
    public ICollection<PokemonNoPool> PokemonNoPool { get; set; } = new List<PokemonNoPool>();
    public ICollection<ItemNoPool> ItensNoPool { get; set; } = new List<ItemNoPool>();
    public ICollection<EscolhaPokemon> Escolhas { get; set; } = new List<EscolhaPokemon>();
    public ICollection<MovimentacaoDraft> Movimentacoes { get; set; } = new List<MovimentacaoDraft>();
    public ICollection<ConfrontoBatalha> Confrontos { get; set; } = new List<ConfrontoBatalha>();
}
