using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Models.Entities;
using Pokedraft.Data;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/conexao")]
public class ConexaoController : ControllerBase
{
    private readonly AppDbContext _context;

    // Tamanho maximo do time de cada jogador numa partida.
    public const int TamanhoMaximoTime = 6;

    public ConexaoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{partidaId}/{jogadorId}")]
    public async Task<IActionResult> GetConexoes(int partidaId, int jogadorId)
    {
        var conexoes = await (
            from c in _context.PokemonsJogadores
            join p in _context.Pokemons on c.PokemonId equals p.Id
            join i in _context.Itens on c.ItemId equals (int?)i.Id into itens
            from item in itens.DefaultIfEmpty()
            where c.PartidaId == partidaId && c.JogadorId == jogadorId
            select new {
                ConexaoId = c.Id,
                p.Id, p.Nome, p.SpriteUrl, p.Custo,
                // tipos usados na tela para colorir a barra de cada Pokémon
                p.PrimeiroTipo, p.SegundoTipo,
                c.ItemId,
                ItemNome = item != null ? item.Nome : null,
                ItemSpriteUrl = item != null ? item.SpriteUrl : null
            }
        ).ToListAsync();

        return Ok(conexoes);
    }

    [HttpPost]
    public async Task<IActionResult> PostConexao([FromBody] Conexao pedido)
    {
        // talvez nem precisasse passar pra outra variaevel
        Conexao c = new();
        c.PartidaId = pedido.PartidaId;
        c.JogadorId = pedido.JogadorId;
        c.PokemonId = pedido.PokemonId;

        var pokemon = await _context.Pokemons.FindAsync(c.PokemonId);
        var jogador = await _context.Jogadores.FindAsync(c.JogadorId);
        var partida = await _context.Partidas.FindAsync(c.PartidaId);
        if(pokemon == null || jogador == null || partida == null)
        {
            return NotFound("Partida, jogador ou pokemon não encontrado(s).");
        }
        if (partida.Turno != pedido.JogadorId)
        {
            return BadRequest("Não é a vez desse jogador.");
        }

        // Trava do tamanho do time: antes dava para comprar mais de 6 Pokémon,
        // porque nem a API nem a tela conferiam a quantidade.
        var quantidadeNoTime = await _context.PokemonsJogadores
            .CountAsync(x => x.PartidaId == c.PartidaId && x.JogadorId == c.JogadorId);
        if (quantidadeNoTime >= TamanhoMaximoTime)
        {
            return BadRequest($"Time cheio: cada jogador pode ter no máximo {TamanhoMaximoTime} Pokémon.");
        }

        var jaTemEstePokemon = await _context.PokemonsJogadores
            .AnyAsync(x => x.PartidaId == c.PartidaId && x.PokemonId == c.PokemonId);
        if (jaTemEstePokemon)
        {
            return BadRequest("Esse Pokémon já foi escolhido nesta partida.");
        }

        if (jogador.Budget - pokemon.Custo < 0)
        {
            return BadRequest("Budget insuficiente.");
        }

        jogador.Budget -= pokemon.Custo;
        pokemon.Disponivel = false;
        _context.PokemonsJogadores.Add(c);
        if(partida.Turno == partida.JogadorUmId)
        {
            partida.Turno = partida.JogadorDoisId;
        } else
        {
            partida.Turno = partida.JogadorUmId;
        }

        await _context.SaveChangesAsync();

        return Ok(c);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConexao(int id)
    {
        var conexao = await _context.PokemonsJogadores.FindAsync(id);
        if (conexao == null)
        {
            return NotFound("Conexão não encontrada.");
        }
        var jogador = await _context.Jogadores.FindAsync(conexao.JogadorId);
        var pokemon = await _context.Pokemons.FindAsync(conexao.PokemonId);
        if (jogador == null || pokemon == null)
        {
            return NotFound("Jogador ou Pokémon não encontrado.");
        }
        jogador.Budget += (int)(pokemon.Custo * 0.75);
        pokemon.Disponivel = true;

        _context.PokemonsJogadores.Remove(conexao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}/{tamanho}")]
    public async Task<IActionResult> VerificarTurno(int id, int tamanho)
    {
        if(tamanho >= 6){
            var turno = await _context.Partidas.FindAsync(id);
            if(turno == null)
            {
                return BadRequest("bad request");
            }
            if(turno.Turno == turno.JogadorUmId)
            {
                turno.Turno = turno.JogadorDoisId;
            } else
            {
                turno.Turno = turno.JogadorUmId;
            }
            await _context.SaveChangesAsync();
        }
        return Ok(" ok ");
    }
}