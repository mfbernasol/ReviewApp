using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult GetPokemons()
    {
        var categories =  _mapper.Map<List<CategoryDto>> (_categoryRepository.GetCategories());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public IActionResult GetCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var category = _mapper.Map<CategoryDto>(_categoryRepository.getCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(category);
    }


    [HttpGet("pokemon/{categoryId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonsByCategoryId(int categoryId)
    {
        var pokemons = _mapper.Map<List<PokemonDto >> (_categoryRepository.GetPokemonByCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(pokemons);
    }


    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        if (categoryCreate == null)
            return BadRequest(ModelState);

        var category = _categoryRepository
            .GetCategories()
            .FirstOrDefault(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper());

        if (category != null)
        {
          ModelState.AddModelError("", "Category already exists");
          return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryCreate);

        if (!_categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
}