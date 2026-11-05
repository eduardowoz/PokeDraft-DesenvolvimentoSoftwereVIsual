namespace Pokedraft.Models.Requests;

public class ResultadoRequest
{
    // Ids dos pokemons escalados, na ordem da batalha, separados por virgula.
    // Ex.: "25,6,9,143,94,130"
    public string? P1 {get; set;}
    public string? P2 {get; set;}
}
