using System.ComponentModel.DataAnnotations;

namespace LivraisonAPI.DTOs.Restaurants;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? ImageUrl { get; set; }
    public double Rating { get; set; }
    public int DeliveryTime { get; set; }
    public bool IsActive { get; set; }
}

public class RestaurantCreateDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? ImageUrl { get; set; }
    public int DeliveryTime { get; set; } = 30;
}

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public int RestaurantId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class DishCreateDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Range(0, 100000)]
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
}

public class CategoryCreateDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
