using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Pokemon> GetPokemons()
    {
        return _context.Pokemon.OrderBy(p => p.Id).ToList(); //explicit about return data type
    }

    public Pokemon GetPokemon(int id)
    {
        return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
    }

    public Pokemon GetPokemon(string name)
    {
        return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
    }

    public decimal GetPokemonRating(int pokeId)
    {
        var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

        if (review.Count() <= 0)
            return 0;
        return ((decimal)review.Sum(r => r.Rating) / review.Count());
    }

    public bool PokemonExists(int pokeId)
    {
        return _context.Pokemon.Any(p => p.Id == pokeId);
    }

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.FirstOrDefault(a => a.Id == ownerId);
        var category = _context.Categories
            .Where(a => a.Id == categoryId).FirstOrDefault();

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon,
        };

        _context.Add(pokemonOwner);

        var pokemonCategory = new PokemonCategory()
        {
            Category = category, 
            Pokemon = pokemon,
        };

        _context.Add(pokemonCategory);

        _context.Add(pokemon);
        
        return Save();
    }

    public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
    {
        return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
    }
    
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0  ? true : false;
    }
}