namespace Pokedraft.Models.Entities;

public class Pokemon
{
    public int Id {get; set;}
    public string? Nome {get; set;}
    public string? PrimeiroTipo {get; set;}
    public string? SegundoTipo {get; set;}
    public int Geracao {get; set;}
    public int Overall {get; set;}
    public int Custo {get; set;}
    public bool Disponivel {get; set;}
    public string? SpriteUrl {get; set;}
}