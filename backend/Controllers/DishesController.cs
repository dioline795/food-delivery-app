using LivraisonAPI.Data;
using LivraisonAPI.DTOs.Restaurants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DishesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DishesController(ApplicationDbContext db)
    {
        _db = db;
    }

    // PUT /api/dishes/{id} — modifier un plat (prix, nom, dispo...)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DishCreateDto dto)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();

        dish.Name = dto.Name;
        dish.Description = dto.Description;
        dish.Price = dto.Price;
        dish.ImageUrl = dto.ImageUrl;
        dish.CategoryId = dto.CategoryId;
        dish.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PATCH /api/dishes/{id}/availability — activer/désactiver un plat rapidement
    [HttpPatch("{id}/availability")]
    public async Task<IActionResult> ToggleAvailability(int id, [FromBody] bool isAvailable)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();

        dish.IsAvailable = isAvailable;
        dish.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/dishes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
