namespace Pokedraft.Models.Entities;

public class Item
{
    public int Id {get; set;}
    public string? Nome {get; set;}
    public int Custo {get; set;}
    public int Overall {get; set;}
    public string? Tipo {get; set;}
    public bool Disponivel {get; set;}
    public string? SpriteUrl {get; set;}
}