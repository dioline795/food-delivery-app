using System.Security.Claims;
using LivraisonAPI.Data;
using LivraisonAPI.DTOs.Orders;
using LivraisonAPI.Models;
using LivraisonAPI.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivraisonAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // toutes les routes de ce contrôleur nécessitent d'être connecté
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public OrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin => User.IsInRole("Admin");

    // POST /api/orders — le client passe commande depuis son panier
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(OrderCreateDto dto)
    {
        var restaurant = await _db.Restaurants.FindAsync(dto.RestaurantId);
        if (restaurant is null) return NotFound(new { message = "Restaurant introuvable." });

        var dishIds = dto.Items.Select(i => i.DishId).ToList();
        var dishes = await _db.Dishes.Where(d => dishIds.Contains(d.Id)).ToListAsync();

        if (dishes.Count != dishIds.Distinct().Count())
            return BadRequest(new { message = "Un ou plusieurs plats sont introuvables." });

        var unavailable = dishes.Where(d => !d.IsAvailable).ToList();
        if (unavailable.Count > 0)
            return BadRequest(new { message = $"Plat(s) indisponible(s) : {string.Join(", ", unavailable.Select(d => d.Name))}" });

        var order = new Order
        {
            UserId = CurrentUserId,
            RestaurantId = dto.RestaurantId,
            DeliveryAddress = dto.DeliveryAddress,
            Notes = dto.Notes,
            Status = OrderStatus.Pending,
        };

        foreach (var item in dto.Items)
        {
            var dish = dishes.First(d => d.Id == item.DishId);
            order.OrderItems.Add(new OrderItem
            {
                DishId = dish.Id,
                Quantity = item.Quantity,
                UnitPrice = dish.Price, // on fige le prix au moment de la commande
            });
        }

        order.TotalPrice = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        var result = await GetOrderDto(order.Id);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, result);
    }

    // GET /api/orders/my — historique des commandes du client connecté
    [HttpGet("my")]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == CurrentUserId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => ToDto(o))
            .ToListAsync();

        return Ok(orders);
    }

    // GET /api/orders — toutes les commandes, réservé à l'admin (panneau d'administration)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        var orders = await _db.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => ToDto(o))
            .ToListAsync();

        return Ok(orders);
    }

    // GET /api/orders/{id} — page de suivi (le client ne voit que ses commandes, l'admin voit tout)
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Dish)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return NotFound();
        if (!IsAdmin && order.UserId != CurrentUserId) return Forbid();

        return Ok(MapFullOrder(order));
    }

    // PATCH /api/orders/{id}/status — l'admin fait avancer la commande dans les 5 statuts
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return NotFound();

        if (!Enum.TryParse<OrderStatus>(dto.Status, true, out var newStatus))
            return BadRequest(new { message = "Statut invalide." });

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // --- Helpers de mapping ---

    private async Task<OrderDto> GetOrderDto(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Dish)
            .FirstAsync(o => o.Id == id);

        return MapFullOrder(order);
    }

    private static OrderDto MapFullOrder(Order order) => new()
    {
        Id = order.Id,
        TotalPrice = order.TotalPrice,
        Status = order.Status.ToString(),
        DeliveryAddress = order.DeliveryAddress,
        Notes = order.Notes,
        CreatedAt = order.CreatedAt,
        UserId = order.UserId,
        UserName = order.User?.Name ?? "",
        RestaurantId = order.RestaurantId,
        RestaurantName = order.Restaurant?.Name ?? "",
        OrderItems = order.OrderItems.Select(oi => new OrderItemDto
        {
            Id = oi.Id,
            DishId = oi.DishId,
            DishName = oi.Dish?.Name ?? "",
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice,
        }).ToList(),
    };

    // Version légère pour les listes (my / admin), sans forcer le chargement de tous les items
    private static OrderDto ToDto(Order o) => new()
    {
        Id = o.Id,
        TotalPrice = o.TotalPrice,
        Status = o.Status.ToString(),
        DeliveryAddress = o.DeliveryAddress,
        Notes = o.Notes,
        CreatedAt = o.CreatedAt,
        UserId = o.UserId,
        UserName = o.User != null ? o.User.Name : "",
        RestaurantId = o.RestaurantId,
        RestaurantName = o.Restaurant != null ? o.Restaurant.Name : "",
    };
}
