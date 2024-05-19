using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IPokemonRepository
{
    ICollection<Pokemon> GetPokemons();
    
    
}