using Pokedraft.Models.Entities;
using Pokedraft.Data;
using Microsoft.EntityFrameworkCore;

namespace Pokedraft.Services;

public class ResultadoService
{
    private readonly AppDbContext _context;

    private readonly List<int> ivs = [0,10,30,60,100,150,210,280,310];
    private readonly List<int> natures = [100,50,0,-50,-100];

    public ResultadoService(AppDbContext context)
    {
        _context = context;
    }

    public class RoundResultado
    {
        public int Rodada {get; set;}
        public Pokemon? PokemonUm {get; set;}
        public Pokemon? PokemonDois {get; set;}
        public string? Vencedor {get; set;}
        public int VencedorId {get; set;}

        // Overall efetivamente usado no confronto (base + IV + natureza + item).
        // Sem isso o front so enxergava o Overall base e o desempate ficava errado.
        public int OverallUm {get; set;}
        public int OverallDois {get; set;}
    }

    public async Task<int> calculoOverall(Pokemon p, int jogadorId, int partidaId)
    {
        int resultado = p.Overall;
        resultado += ivs[Random.Shared.Next(ivs.Count)];
        resultado += natures[Random.Shared.Next(natures.Count)];

        // Conexao nao tem propriedade de navegacao (so o int? ItemId), por isso o
        // .Include(c => c.ItemId) quebrava: Include so aceita navegacao, nao escalar.
        // Busca direta resolve sem precisar mexer no modelo.
        var conexao = await _context.PokemonsJogadores
            .FirstOrDefaultAsync(c =>
                c.PartidaId == partidaId &&
                c.JogadorId == jogadorId &&
                c.PokemonId == p.Id
            );

        // sem conexao ou sem item equipado => fica so com base + IV + natureza
        if (conexao?.ItemId == null)
        {
            return resultado;
        }

        var item = await _context.Itens.FindAsync(conexao.ItemId);
        if (item == null)
        {
            return resultado;
        }

        // Item.Overall funciona como "tier" do item: cada tier vale 10% do overall.
        //   plates tipados  -> tier 1, custam 10, so valem se o tipo casar
        //   Leftovers etc.  -> tier 2/3, custam 30/60, sem tipo, valem sempre
        bool semTipo = string.IsNullOrEmpty(item.Tipo);
        bool tipoCasa =
            !semTipo && (
                string.Equals(p.PrimeiroTipo, item.Tipo, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(p.SegundoTipo,  item.Tipo, StringComparison.OrdinalIgnoreCase)
            );

        if (semTipo || tipoCasa)
        {
            resultado += resultado * item.Overall / 10;
        }

        return resultado;
    }

    public async Task<List<RoundResultado>> SimularConfrontos(
        List<Pokemon> pokemonsUm, List<Pokemon> pokemonsDois,
        int idJogadorUm, int idJogadorDois,
        string nomeJogadorUm, string nomeJogadorDois,
        int partidaId
    )
    {
        List<RoundResultado> resultados = new();
        int totalRounds = Math.Min(pokemonsUm.Count, pokemonsDois.Count);

        for (int i = 0; i < totalRounds; i++)
        {
            var p1 = pokemonsUm[i];
            var p2 = pokemonsDois[i];
            string vencedor = "Empate";
            int vencedorId = 0;

            var overallUm = await calculoOverall(p1, idJogadorUm, partidaId);
            var overallDois = await calculoOverall(p2, idJogadorDois, partidaId);

            if (overallUm > overallDois)
            {
                vencedor = nomeJogadorUm;
                vencedorId = idJogadorUm;
            }
            else if (overallDois > overallUm)
            {
                vencedor = nomeJogadorDois;
                vencedorId = idJogadorDois;
            }

            resultados.Add(new RoundResultado
            {
                Rodada = i + 1,
                PokemonUm = p1, PokemonDois = p2,
                Vencedor = vencedor, VencedorId = vencedorId,
                OverallUm = overallUm, OverallDois = overallDois
            });
        }

        return resultados;
    }
}
