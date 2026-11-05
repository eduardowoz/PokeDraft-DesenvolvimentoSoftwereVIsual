using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Data;
using Pokedraft.Models.Entities;
using Pokedraft.Models.Requests;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/partida")]
public class PartidaController : ControllerBase
{
    private readonly AppDbContext _context;
    public PartidaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPartida(int id)
    {
        var p = await _context.Partidas.FindAsync(id);
        if(p == null)
        {
            return NotFound();
        }
        return Ok(p);
    }

    // Budget minimo de uma partida. A tela tambem valida, mas a regra que vale
    // e esta: sem ela daria para criar partida com 1 ponto chamando a API direto.
    public const int BudgetMinimo = 1000;

    [HttpPost]
    public async Task<IActionResult> PostPartida([FromBody] PartidaRequest pedido)
    {
        if (pedido == null)
        {
            return BadRequest("Dados da partida não informados.");
        }
        if (pedido.Budget < BudgetMinimo)
        {
            return BadRequest($"Budget inferior ao mínimo: informe ao menos {BudgetMinimo} pts.");
        }
        if (pedido.JogadorUmId == pedido.JogadorDoisId)
        {
            return BadRequest("Escolha dois jogadores diferentes para a partida.");
        }

        var jogadorUm = await _context.Jogadores.FindAsync(pedido.JogadorUmId);
        var jogadorDois = await _context.Jogadores.FindAsync(pedido.JogadorDoisId);
        if (jogadorUm == null || jogadorDois == null)
        {
            return NotFound("Um ou ambos jogadores não foram encontrados.");
        }

        Partida p = new();
        p.JogadorUmId = pedido.JogadorUmId;
        p.JogadorDoisId = pedido.JogadorDoisId;
        p.Budget = pedido.Budget;

        _context.Partidas.Add(p);
        await _context.SaveChangesAsync();

        return Created("Partida criada.", p);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchBudget(int id)
    {
        var partida = await _context.Partidas.FindAsync(id);
        if(partida == null)
        {
            return NotFound("Partida não encontrada.");
        }
        if(partida.Start)
        {
            return Ok("Os orçamentos já foram inicializados.");
        }

        var jogadorUm = await _context.Jogadores.FindAsync(partida.JogadorUmId);
        var jogadorDois = await _context.Jogadores.FindAsync(partida.JogadorDoisId);
        if (jogadorUm == null || jogadorDois == null)
        {
            return BadRequest("Um ou ambos jogadores não foram encontrados.");
        }

        jogadorUm.Budget = partida.Budget;
        jogadorDois.Budget = partida.Budget;
        partida.Turno = partida.JogadorUmId;
        partida.Start = true;

        await _context.SaveChangesAsync();

        return Ok("Orçamentos inicializados com sucesso.");
    }

    [HttpPost("resetar")]
    public async Task<IActionResult> ResetData()
    {
        var pokemons = await _context.Pokemons.ToListAsync();
        foreach (var pokemon in pokemons)
        {
            pokemon.Disponivel = true;
        }

        var itens = await _context.Itens.ToListAsync();
        foreach (var item in itens)
        {
            item.Disponivel = true;
        }

        var conexoes = await _context.PokemonsJogadores.ToListAsync();
        _context.PokemonsJogadores.RemoveRange(conexoes);

        var partidas = await _context.Partidas.ToListAsync();
        _context.Partidas.RemoveRange(partidas);

        await _context.SaveChangesAsync();

        return Ok("Banco de dados resetado com sucesso.");
    }
}