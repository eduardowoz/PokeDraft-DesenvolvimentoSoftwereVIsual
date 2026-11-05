using Microsoft.EntityFrameworkCore;
using Pokedraft.Data;
using Pokedraft.Models.Entities;

namespace Pokedraft.Services;

public class DraftService
{
    private readonly AppDbContext _context;
    private readonly List<string> tipo = [
        "normal",
        "fighting",
        "flying",
        "poison",
        "ground",
        "rock",
        "bug",
        "ghost",
        "steel",
        "fire",
        "water",
        "grass",
        "electric",
        "psychic",
        "ice",
        "dragon",
        "dark",
        "fairy"
    ];
    private readonly List<int> geracao = [1,2,3,4,5,6,7,8,9];

    public DraftService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pokemon>> fazerDraft(int PartidaId)
    {
        var partida = await _context.Partidas.FindAsync(PartidaId);
        if(partida == null)
        {
            throw new KeyNotFoundException("Partida não encontrada.");
        }
        if(partida.Tipo == null || partida.Geracao == null)
        {
            partida.Tipo = tipo[Random.Shared.Next(tipo.Count())];
            partida.Geracao = geracao[Random.Shared.Next(geracao.Count())];
            await _context.SaveChangesAsync();
        }
        var tipoPokemon = partida.Tipo;
        var geracaoPokemon = partida.Geracao;

        var pokemons = (
            await _context.Pokemons
            .Where(
                p => p.Geracao == geracaoPokemon
                && (p.PrimeiroTipo == tipoPokemon
                || p.SegundoTipo == tipoPokemon)
                && p.Disponivel == true
            )
            .ToListAsync()
        );

        return pokemons.OrderBy(_ => Random.Shared.Next()).ToList();
    }

    public async Task<List<Pokemon>> girarGeracao(int IdPartida)
    {
        var partida = await _context.Partidas.FindAsync(IdPartida);
        if(partida == null)
        {
            throw new InvalidOperationException("Partida inexistente");
        }
        int? confirmacao = partida.Geracao;
        while(confirmacao == partida.Geracao){
            partida.Geracao = geracao[Random.Shared.Next(geracao.Count())];
        };
        await _context.SaveChangesAsync();
        return await fazerDraft(IdPartida);
    }

    public async Task<List<Pokemon>> girarTipo(int IdPartida)
    {
        var partida = await _context.Partidas.FindAsync(IdPartida);
        if(partida == null)
        {
            throw new InvalidOperationException("Partida inexistente");
        }
        string? confirmacao = partida.Tipo;
        while(confirmacao == partida.Tipo){
            partida.Tipo = tipo[Random.Shared.Next(tipo.Count())];
        };
        await _context.SaveChangesAsync();
        return await fazerDraft(IdPartida);
    }
}