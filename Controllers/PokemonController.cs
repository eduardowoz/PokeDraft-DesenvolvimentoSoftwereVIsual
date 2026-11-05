using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Models.Entities;
using Pokedraft.Data;
using Pokedraft.Services;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/pokemon")]
public class PokemonController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PublicService _service;
    private readonly DraftService _draft;

    public PokemonController(AppDbContext context, PublicService service, DraftService draft)
    {
        _context = context;
        _service = service;
        _draft = draft;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPokemons(int id)
    {
        var resultado = await _draft.fazerDraft(id);
        return Ok(resultado);
    }

    [HttpPost("geracao/{id}")]
    public async Task<IActionResult> GetPokemonsGeracao(int id)
    {
        var resultado = await _draft.girarGeracao(id);
        return Ok(resultado);
    }
    
    [HttpPost("tipo/{id}")]
    public async Task<IActionResult> GetPokemonsTipo(int id)
    {
        var resultado = await _draft.girarTipo(id);
        return Ok(resultado);
    }

    [HttpPost("{id1}/{id2}")]
    public async Task<IActionResult> PostPokemons(int id1, int id2)
    {
        for(int i=id1; i<id2; i++)
        {
            var resultado = await _service.BuscarPokemonAsync(i);

            Pokemon p = new();
            p.Id = resultado.Id;
            p.Nome = resultado.Nome;
            p.PrimeiroTipo = resultado.Tipos[0].Tipo.Nome;
            if(resultado.Tipos.Count > 1)
            {
                p.SegundoTipo = resultado.Tipos[1].Tipo.Nome;
            }
            p.Overall = 0;
            for(int j=0; j<6; j++){
                p.Overall += resultado.BaseStats[j].Base;
            }
            p.Custo = p.Overall/6;
            p.Disponivel = resultado.Disponivel;
            p.SpriteUrl = resultado.SpriteUrl.Imagem;
            if(resultado.Id <= 151)
            {
                p.Geracao = 1;
            } else if(resultado.Id <= 251)
            {
                p.Geracao = 2;
            } else if(resultado.Id <= 386)
            {
                p.Geracao = 3;
            } else if(resultado.Id <= 494)
            {
                p.Geracao = 4;
            } else if(resultado.Id <= 649)
            {
                p.Geracao = 5;
            } else if(resultado.Id <= 721)
            {
                p.Geracao = 6;
            } else if(resultado.Id <= 809)
            {
                p.Geracao = 7;
            } else if(resultado.Id <= 905)
            {
                p.Geracao = 8;
            } else if(resultado.Id <= 1025)
            {
                p.Geracao = 9;
            }
            _context.Pokemons.Add(p);
        }
        await _context.SaveChangesAsync();
        return Ok();
    }
}