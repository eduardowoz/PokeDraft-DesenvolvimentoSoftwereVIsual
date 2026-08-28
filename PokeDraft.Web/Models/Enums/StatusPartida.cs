namespace PokeDraft.Web.Models.Enums;

/// <summary>
/// Etapas pelas quais uma partida passa, do início ao fim.
/// </summary>
public enum StatusPartida
{
    /// <summary>Partida criada; jogadores e configuração sendo definidos.</summary>
    Configuracao = 1,

    /// <summary>Pools sorteados; jogadores banindo, comprando e vendendo Pokémon/itens.</summary>
    Draft = 2,

    /// <summary>Draft encerrado; jogadores ordenando o time e simulando a batalha.</summary>
    Batalha = 3,

    /// <summary>Batalha simulada; vencedor definido.</summary>
    Finalizada = 4
}
