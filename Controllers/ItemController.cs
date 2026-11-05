using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedraft.Models.Entities;
using Pokedraft.Services;
using Pokedraft.Data;

namespace Pokedraft.Controllers;

[ApiController]
[Route("api/item")]
public class ItemController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ItemService _itemService;

    public ItemController(AppDbContext context, ItemService itemService)
    {
        _context = context;
        _itemService = itemService;
    }

    [HttpGet("{partidaId}")]
    public async Task<IActionResult> GetItensDisponiveis(int partidaId)
    {
        var itens = await _itemService.carregarItensDisponiveis(partidaId);
        if(itens == null)
        {
            return NotFound();
        }

        return Ok(itens);
    }

    [HttpPost]
    public async Task<IActionResult> PostItem([FromBody] Item i)
    {
        i.Disponivel = true;
        _context.Itens.Add(i);

        await _context.SaveChangesAsync();

        return Ok(i);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, [FromBody] Item i)
    {
        var item = await _context.Itens.FindAsync(id);
        if (item == null)
        {
            return NotFound("Item não encontrado.");
        }

        item.Nome = i.Nome;
        item.Custo = i.Custo;
        item.Overall = i.Overall;
        item.Tipo = i.Tipo;
        item.SpriteUrl = i.SpriteUrl;
        item.Disponivel = i.Disponivel;

        await _context.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPost("{partidaId}/{jogadorId}/{conexaoId}/{itemId}")]
    public async Task<IActionResult> ComprarItem(
        int partidaId, int jogadorId, int conexaoId, int itemId
    )
    {
        try
        {
            await _itemService.comprarItem(
                partidaId, jogadorId, conexaoId, itemId
            );

            return NoContent();
        }
        catch (KeyNotFoundException erro)
        {
            return NotFound(erro.Message);
        }
        catch (InvalidOperationException erro)
        {
            return BadRequest(erro.Message);
        }
    }
}