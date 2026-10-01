using System.ComponentModel.DataAnnotations;

namespace LivraisonAPI.DTOs.Orders;

public class OrderItemCreateDto
{
    [Required]
    public int DishId { get; set; }

    [Range(1, 50)]
    public int Quantity { get; set; }
}

public class OrderCreateDto
{
    [Required]
    public int RestaurantId { get; set; }

    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;

    public string? Notes { get; set; }

    [Required, MinLength(1)]
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public string DishName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? DeliveryAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
