using System.ComponentModel.DataAnnotations.Schema;

namespace LivraisonAPI.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    // Prix figé au moment de la commande (le prix du plat peut changer après)
    [Column(TypeName = "numeric(10,2)")]
    public decimal UnitPrice { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int DishId { get; set; }
    public Dish? Dish { get; set; }
}
