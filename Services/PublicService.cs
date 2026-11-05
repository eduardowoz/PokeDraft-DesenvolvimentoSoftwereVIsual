using System.Text.Json;
using Pokedraft.Models.Dto;

namespace Pokedraft.Services;

public class PublicService
{
    private readonly HttpClient _cliente;
    public PublicService(IHttpClientFactory factory)
    {
        _cliente = factory.CreateClient();
    }

    public async Task<PokemonDto?> BuscarPokemonAsync(int id)
    {
        var resposta = await _cliente.GetAsync(
            $"https://pokeapi.co/api/v2/pokemon/{id}"
        );
        if (!resposta.IsSuccessStatusCode){
            return null;
        }
        var conteudo = await resposta.Content.ReadAsStringAsync();
        var pokemon = JsonSerializer.Deserialize<PokemonDto>(conteudo);
        return pokemon;
    }
}