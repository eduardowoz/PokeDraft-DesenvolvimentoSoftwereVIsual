using System.Text.Json.Serialization;

namespace Pokedraft.Models.Dto;

public class PokemonDto
{
    [JsonPropertyName("id")]
    public int Id {get; set;}
    [JsonPropertyName("name")]
    public string? Nome {get; set;}
    [JsonPropertyName("is_default")]
    public bool Disponivel {get; set;}
    [JsonPropertyName("types")]
    public List<PokemonTypeDto> Tipos {get; set;} = new();
    [JsonPropertyName("stats")]
    public List<Stats> BaseStats {get; set;} = new();
    [JsonPropertyName("sprites")]
    public SpritesDto? SpriteUrl {get; set;}
}

public class Stats
{
    [JsonPropertyName("base_stat")]
    public int Base {get; set;}
}

public class PokemonTypeDto
{
    [JsonPropertyName("slot")]
    public int Slot {get; set;}

    [JsonPropertyName("type")]
    public TipoDto Tipo {get; set;} = new();
}

public class TipoDto
{
    [JsonPropertyName("name")]
    public string? Nome {get; set;}
}

public class SpritesDto
{
    [JsonPropertyName("front_default")]
    public string? Imagem {get; set;}
}