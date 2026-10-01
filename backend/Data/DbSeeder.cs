using LivraisonAPI.Models;
using LivraisonAPI.Models.Enums;

namespace LivraisonAPI.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // ===== ADMIN =====
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@livraison.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin,
            });
        }

        // ===== RESTAURANTS =====
        if (!db.Restaurants.Any())
        {
            var pizzeria = new Restaurant
            {
                Name = "Bella Pizza",
                Description = "Pizzas italiennes authentiques au feu de bois",
                Address = "ул. Ленина, 10",
                Phone = "+7 900 000-00-01",
                ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=600&h=400&fit=crop",
                Rating = 4.8,
                DeliveryTime = 30,
                IsActive = true,
            };

            var sushi = new Restaurant
            {
                Name = "Sakura Sushi",
                Description = "Sushis et cuisine japonaise fraîche",
                Address = "пр. Мира, 25",
                Phone = "+7 900 000-00-02",
                ImageUrl = "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=600&h=400&fit=crop",
                Rating = 4.6,
                DeliveryTime = 40,
                IsActive = true,
            };

            var burger = new Restaurant
            {
                Name = "Burger House",
                Description = "Burgers juteux et frites maison",
                Address = "ул. Пушкина, 5",
                Phone = "+7 900 000-00-03",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&h=400&fit=crop",
                Rating = 4.5,
                DeliveryTime = 25,
                IsActive = true,
            };

            db.Restaurants.AddRange(pizzeria, sushi, burger);
            await db.SaveChangesAsync();

            // ===== CATÉGORIES =====
            var pizzaCat = new Category { Name = "Pizzas", RestaurantId = pizzeria.Id };
            var dessertCat = new Category { Name = "Desserts", RestaurantId = pizzeria.Id };
            var rollsCat = new Category { Name = "Rolls", RestaurantId = sushi.Id };
            var soupCat = new Category { Name = "Soupes", RestaurantId = sushi.Id };
            var burgerCat = new Category { Name = "Burgers", RestaurantId = burger.Id };
            var friesCat = new Category { Name = "Frites", RestaurantId = burger.Id };

            db.Categories.AddRange(pizzaCat, dessertCat, rollsCat, soupCat, burgerCat, friesCat);
            await db.SaveChangesAsync();

            // ===== PLATS =====
            db.Dishes.AddRange(
                // Bella Pizza
                new Dish
                {
                    Name = "Margherita",
                    Description = "Tomate, mozzarella, basilic",
                    Price = 590,
                    RestaurantId = pizzeria.Id,
                    CategoryId = pizzaCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=300",
                },
                new Dish
                {
                    Name = "Pepperoni",
                    Description = "Tomate, mozzarella, pepperoni",
                    Price = 690,
                    RestaurantId = pizzeria.Id,
                    CategoryId = pizzaCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=300",
                },
                new Dish
                {
                    Name = "Tiramisu",
                    Description = "Dessert italien classique",
                    Price = 350,
                    RestaurantId = pizzeria.Id,
                    CategoryId = dessertCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=300",
                },

                // Sakura Sushi
                new Dish
                {
                    Name = "California Roll",
                    Description = "Avocat, crabe, concombre",
                    Price = 450,
                    RestaurantId = sushi.Id,
                    CategoryId = rollsCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=300",
                },
                new Dish
                {
                    Name = "Philadelphia Roll",
                    Description = "Saumon, fromage frais",
                    Price = 490,
                    RestaurantId = sushi.Id,
                    CategoryId = rollsCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1617196034796-73dfa7b1fd56?w=300",
                },
                new Dish
                {
                    Name = "Soupe Miso",
                    Description = "Soupe traditionnelle japonaise",
                    Price = 250,
                    RestaurantId = sushi.Id,
                    CategoryId = soupCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1547592180-85f173990554?w=300",
                },

                // Burger House
                new Dish
                {
                    Name = "Cheeseburger",
                    Description = "Bœuf, cheddar, salade, tomate",
                    Price = 550,
                    RestaurantId = burger.Id,
                    CategoryId = burgerCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=300",
                },
                new Dish
                {
                    Name = "Double Burger",
                    Description = "Double bœuf, double cheddar",
                    Price = 750,
                    RestaurantId = burger.Id,
                    CategoryId = burgerCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=300",
                },
                new Dish
                {
                    Name = "Frites maison",
                    Description = "Frites croustillantes",
                    Price = 200,
                    RestaurantId = burger.Id,
                    CategoryId = friesCat.Id,
                    IsAvailable = true,
                    ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=300",
                }
            );
        }

        await db.SaveChangesAsync();
    }
}