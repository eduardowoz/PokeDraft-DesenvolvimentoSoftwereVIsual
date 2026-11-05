namespace Pokedraft.Models.Entities;

public class Partida
{
    public int Id {get; set;}
    public int JogadorUmId {get; set;}
    public int JogadorDoisId {get; set;}
    public int Budget {get; set;}
    public string? Tipo {get; set;}
    public int? Geracao {get; set;}
    public bool Start {get; set;} = false;
    public int? Turno {get; set;}
}