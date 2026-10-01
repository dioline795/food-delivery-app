using System.ComponentModel.DataAnnotations;

namespace LivraisonAPI.Models;

public class Restaurant
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Address { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    public double Rating { get; set; } = 0;

    // Temps de livraison estimé en minutes
    public int DeliveryTime { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
