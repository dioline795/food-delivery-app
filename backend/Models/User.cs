using System.ComponentModel.DataAnnotations;
using LivraisonAPI.Models.Enums;

namespace LivraisonAPI.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    // Hash bcrypt du mot de passe (jamais le mot de passe en clair)
    [Required, MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public UserRole Role { get; set; } = UserRole.Client;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
