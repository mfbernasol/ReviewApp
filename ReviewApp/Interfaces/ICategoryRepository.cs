using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ICategoryRepository
{
    ICollection<Category> GetCategories();
    Category getCategory(int id);
    ICollection<Pokemon> GetPokemonByCategory(int categoryId);
    bool CategoryExists(int id);
    bool CreateCategory(Category category);
    bool Save();
}