namespace PokeDraft.Web.Models.Enums;

/// <summary>
/// Cada ação que um jogador realiza durante o draft gera uma movimentação no histórico.
/// </summary>
public enum TipoMovimentacao
{
    /// <summary>Jogador baniu um Pokémon do pool (ninguém pode escolhê-lo).</summary>
    Banimento = 1,

    /// <summary>Jogador gastou pontos para adicionar um Pokémon ao time.</summary>
    CompraPokemon = 2,

    /// <summary>Jogador vendeu um Pokémon do time e recuperou parte dos pontos.</summary>
    VendaPokemon = 3,

    /// <summary>Jogador equipou um item em um Pokémon do time.</summary>
    EquiparItem = 4,

    /// <summary>Jogador removeu o item de um Pokémon do time.</summary>
    RemoverItem = 5
}
