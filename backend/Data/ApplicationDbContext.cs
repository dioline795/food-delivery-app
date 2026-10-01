using LivraisonAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LivraisonAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Contraintes d'unicité ---
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // --- Enums stockés en texte lisible (ex: "Pending", "Delivered") plutôt qu'en entier ---
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // --- Relations : User (1) -> Orders (N) ---
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict); // on ne supprime pas les commandes si l'user est supprimé

        // --- Relations : Restaurant (1) -> Orders (N) ---
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Relations : Restaurant (1) -> Categories (N) ---
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Restaurant)
            .WithMany(r => r.Categories)
            .HasForeignKey(c => c.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- Relations : Restaurant (1) -> Dishes (N) ---
        modelBuilder.Entity<Dish>()
            .HasOne(d => d.Restaurant)
            .WithMany(r => r.Dishes)
            .HasForeignKey(d => d.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- Relations : Category (1) -> Dishes (N) ---
        modelBuilder.Entity<Dish>()
            .HasOne(d => d.Category)
            .WithMany(c => c.Dishes)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Relations : Order (1) -> OrderItems (N) ---
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // si on supprime la commande, ses lignes partent avec

        // --- Relations : Dish (1) -> OrderItems (N) ---
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Dish)
            .WithMany(d => d.OrderItems)
            .HasForeignKey(oi => oi.DishId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
