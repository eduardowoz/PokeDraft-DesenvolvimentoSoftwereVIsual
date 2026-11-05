using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Data;
using Pokedraft.Models.Entities;
using Pokedraft.Models.Requests;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/jogador")]
public class JogadorController : ControllerBase
{
    private readonly AppDbContext _context;
    public JogadorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetJogadores()
    {
        var jogadores = await _context.Jogadores.ToListAsync();
        return Ok(jogadores);
    }

    [HttpPost]
    public async Task<IActionResult> PostJogador([FromBody] JogadorRequest pedido)
    {
        if(await _context.Jogadores.AnyAsync(i => i.Nome == pedido.Nome))
        {
            return Conflict("Já existe um jogador com esse nome.");
        }

        Jogador j = new();
        j.Nome = pedido.Nome;
        j.SpriteUrl = pedido.SpriteUrl;
        _context.Jogadores.Add(j);

        await _context.SaveChangesAsync();

        return Created("Jogador criado.", j);
    }

    [HttpGet("sprites")]
    public IActionResult GetSprites()
    {
        string pasta = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "images", "players"
        );

        var sprites = Directory.GetFiles(pasta)
            .Where(arquivo =>
                arquivo.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                || arquivo.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                || arquivo.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                || arquivo.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
            ).Select(arquivo =>
                "/images/players/" + Path.GetFileName(arquivo)
            ).ToList();
            
        return Ok(sprites);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJogador(int id)
    {
        var jogador = await _context.Jogadores.FindAsync(id);
        if (jogador == null)
        {
            return NotFound("Jogador nao encontrado.");
        }

        _context.Jogadores.Remove(jogador);
        await _context.SaveChangesAsync();

        return Ok("Jogador removido com sucesso.");
    }
}