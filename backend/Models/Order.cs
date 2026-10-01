using System.ComponentModel.DataAnnotations.Schema;
using LivraisonAPI.Models.Enums;

namespace LivraisonAPI.Models;

public class Order
{
    public int Id { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public string? DeliveryAddress { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User? User { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
