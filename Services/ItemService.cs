using Microsoft.EntityFrameworkCore;
using Pokedraft.Data;
using Pokedraft.Models.Entities;

namespace Pokedraft.Services;

public class ItemService
{
    private readonly AppDbContext _context;

    public ItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> carregarItensDisponiveis(int partidaId)
    {
        var partida = await _context.Partidas.FindAsync(partidaId);
        if (partida == null)
        {
            throw new KeyNotFoundException("Partida não encontrada.");
        }

        var itensUsados = await _context.PokemonsJogadores
            .Where(c => c.PartidaId == partidaId && c.ItemId != null)
            .Select(c => c.ItemId!.Value)
            .ToListAsync();

        var itensDisponiveis = await _context.Itens
            .Where(i => i.Disponivel && !itensUsados.Contains(i.Id))
            .ToListAsync();

        return itensDisponiveis;
    }

    public async Task comprarItem(
        int partidaId, int jogadorId, int conexaoId, int itemId
    )
    {
        var partida = await _context.Partidas.FindAsync(partidaId);
        if (partida == null)
        {
            throw new KeyNotFoundException("Partida não encontrada.");
        }
        if (partida.Turno != jogadorId)
        {
            throw new InvalidOperationException("Não é a vez desse jogador.");
        }

        var jogador = await _context.Jogadores.FindAsync(jogadorId);
        var conexao = await _context.PokemonsJogadores.FindAsync(conexaoId);
        var item = await _context.Itens.FindAsync(itemId);
        if (jogador == null || conexao == null || item == null)
        {
            throw new KeyNotFoundException("Jogador, conexão ou item não encontrado.");
        }
        if (conexao.PartidaId != partidaId || conexao.JogadorId != jogadorId)
        {
            throw new InvalidOperationException(
                "Algo deu errado."
            );
        }
        if (conexao.ItemId != null)
        {
            throw new InvalidOperationException("Esse Pokémon já possui um item.");
        }
        if (!item.Disponivel)
        {
            throw new InvalidOperationException("Esse item não está disponível.");
        }

        var itemJaComprado = await _context.PokemonsJogadores
            .AnyAsync(c => c.PartidaId == partidaId && c.ItemId == itemId);
        if (itemJaComprado)
        {
            throw new InvalidOperationException("Esse item já foi escolhido nesta partida.");
        }
        if (jogador.Budget < item.Custo)
        {
            throw new InvalidOperationException("Budget insuficiente.");
        }

        jogador.Budget -= item.Custo;
        conexao.ItemId = item.Id;
        item.Disponivel = false;

        if(partida.Turno == partida.JogadorUmId)
        {
            partida.Turno = partida.JogadorDoisId;
        } else
        {
            partida.Turno = partida.JogadorUmId;
        }

        await _context.SaveChangesAsync();
    }
}