using LivraisonAPI.Data;
using LivraisonAPI.DTOs.Restaurants;
using LivraisonAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivraisonAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RestaurantsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/restaurants — public, liste des restaurants actifs (page d'accueil)
    [HttpGet]
    public async Task<ActionResult<List<RestaurantDto>>> GetAll()
    {
        var restaurants = await _db.Restaurants
            .Where(r => r.IsActive)
            .Select(r => ToDto(r))
            .ToListAsync();

        return Ok(restaurants);
    }

    // GET /api/restaurants/{id} — public, détail d'un restaurant
    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();
        return Ok(ToDto(restaurant));
    }

    // GET /api/restaurants/{id}/dishes — public, menu complet (utilisé par la page menu du front)
    [HttpGet("{id}/dishes")]
    public async Task<ActionResult<List<DishDto>>> GetDishes(int id)
    {
        var dishes = await _db.Dishes
            .Include(d => d.Category)
            .Where(d => d.RestaurantId == id)
            .Select(d => new DishDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl,
                IsAvailable = d.IsAvailable,
                RestaurantId = d.RestaurantId,
                CategoryId = d.CategoryId,
                CategoryName = d.Category != null ? d.Category.Name : "",
            })
            .ToListAsync();

        return Ok(dishes);
    }

    // GET /api/restaurants/{id}/categories — public
    [HttpGet("{id}/categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories(int id)
    {
        var categories = await _db.Categories
            .Where(c => c.RestaurantId == id)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, RestaurantId = c.RestaurantId })
            .ToListAsync();

        return Ok(categories);
    }

    // POST /api/restaurants — admin uniquement
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RestaurantDto>> Create(RestaurantCreateDto dto)
    {
        var restaurant = new Restaurant
        {
            Name = dto.Name,
            Description = dto.Description,
            Address = dto.Address,
            Phone = dto.Phone,
            ImageUrl = dto.ImageUrl,
            DeliveryTime = dto.DeliveryTime,
            IsActive = true,
        };

        _db.Restaurants.Add(restaurant);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, ToDto(restaurant));
    }

    // PUT /api/restaurants/{id} — admin uniquement
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, RestaurantCreateDto dto)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();

        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.Address = dto.Address;
        restaurant.Phone = dto.Phone;
        restaurant.ImageUrl = dto.ImageUrl;
        restaurant.DeliveryTime = dto.DeliveryTime;
        restaurant.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/restaurants/{id} — admin uniquement (désactivation plutôt que suppression physique)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();

        restaurant.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/restaurants/{id}/categories — admin uniquement
    [HttpPost("{id}/categories")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> CreateCategory(int id, CategoryCreateDto dto)
    {
        var restaurantExists = await _db.Restaurants.AnyAsync(r => r.Id == id);
        if (!restaurantExists) return NotFound(new { message = "Restaurant introuvable." });

        var category = new Category { Name = dto.Name, RestaurantId = id };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return Ok(new CategoryDto { Id = category.Id, Name = category.Name, RestaurantId = id });
    }

    // POST /api/restaurants/{id}/dishes — admin uniquement
    [HttpPost("{id}/dishes")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DishDto>> CreateDish(int id, DishCreateDto dto)
    {
        var restaurantExists = await _db.Restaurants.AnyAsync(r => r.Id == id);
        if (!restaurantExists) return NotFound(new { message = "Restaurant introuvable." });

        var dish = new Dish
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            RestaurantId = id,
            IsAvailable = true,
        };

        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync();

        return Ok(new DishDto
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            ImageUrl = dish.ImageUrl,
            IsAvailable = dish.IsAvailable,
            RestaurantId = dish.RestaurantId,
            CategoryId = dish.CategoryId,
        });
    }

    private static RestaurantDto ToDto(Restaurant r) => new()
    {
        Id = r.Id,
        Name = r.Name,
        Description = r.Description,
        Address = r.Address,
        Phone = r.Phone,
        ImageUrl = r.ImageUrl,
        Rating = r.Rating,
        DeliveryTime = r.DeliveryTime,
        IsActive = r.IsActive,
    };
}
