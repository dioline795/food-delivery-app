using System.ComponentModel.DataAnnotations;

namespace LivraisonAPI.Models;

public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
