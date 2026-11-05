using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Data;
using Pokedraft.Models.Requests;
using Pokedraft.Services;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/resultado")]
public class ResultadoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ResultadoService _resultadoService;

    public ResultadoController(AppDbContext context, ResultadoService resultadoService)
    {
        _context = context;
        _resultadoService = resultadoService;
    }

    // Virou POST porque agora recebe os times no corpo ([FromBody]) e porque
    // simular a batalha nao e idempotente (IV e natureza sao sorteados a cada chamada).
    [HttpPost("{partidaId}")]
    public async Task<IActionResult> GetResultado(
        int partidaId, [FromBody] ResultadoRequest pedido
    )
    {
        var partida = await _context.Partidas.FindAsync(partidaId);
        if (partida == null)
        {
            return NotFound("Partida não encontrada.");
        }

        var jogadorUm = await _context.Jogadores.FindAsync(partida.JogadorUmId);
        var jogadorDois = await _context.Jogadores.FindAsync(partida.JogadorDoisId);
        if (jogadorUm == null || jogadorDois == null)
        {
            return NotFound("Jogadores não encontrados.");
        }
        if (pedido == null || string.IsNullOrEmpty(pedido.P1) || string.IsNullOrEmpty(pedido.P2))
        {
            return BadRequest("Pokémon não informados para a batalha.");
        }

        // TryParse no lugar do Parse: id invalido virava excecao 500 em vez de 400
        if (!TentarLerIds(pedido.P1, out var idsUm) || !TentarLerIds(pedido.P2, out var idsDois))
        {
            return BadRequest("Lista de Pokémon inválida.");
        }

        var pokemonsUm = await _context.Pokemons.Where(p => idsUm.Contains(p.Id)).ToListAsync();
        var pokemonsDois = await _context.Pokemons.Where(p => idsDois.Contains(p.Id)).ToListAsync();

        // isso aqui é para ordenar os pokemons na ordem escolhida lá na batalha
        // (o First estourava se um id nao existisse no banco; agora devolve 400)
        if (pokemonsUm.Count != idsUm.Count || pokemonsDois.Count != idsDois.Count)
        {
            return BadRequest("Algum Pokémon escalado não existe no banco.");
        }
        pokemonsUm = idsUm.Select(id => pokemonsUm.First(p => p.Id == id)).ToList();
        pokemonsDois = idsDois.Select(id => pokemonsDois.First(p => p.Id == id)).ToList();

        var resultado = await _resultadoService.SimularConfrontos(
            pokemonsUm, pokemonsDois,
            jogadorUm.Id, jogadorDois.Id,
            // ?? *traduz para* Se nulo entao
            jogadorUm.Nome ?? "Jogador 1", jogadorDois.Nome ?? "Jogador 2",
            partida.Id
        );

        return Ok(new {
            JogadorUm = jogadorUm,
            JogadorDois = jogadorDois,
            Rodadas = resultado
        });
    }

    private static bool TentarLerIds(string texto, out List<int> ids)
    {
        ids = new List<int>();
        foreach (var parte in texto.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!int.TryParse(parte.Trim(), out var id))
            {
                return false;
            }
            ids.Add(id);
        }
        return ids.Count > 0;
    }
}
