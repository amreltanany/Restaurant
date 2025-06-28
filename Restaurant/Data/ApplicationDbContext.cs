using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Data
{
    // ApplicationDbContext inherits from IdentityDbContext to include Identity,
    // IdentityDbContext provides the necessary functionality for user management,
    // ApplicationUser is the custom user class that extends IdentityUser
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that accepts DbContextOptions and passes it to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        //ProductIngredient is a join table for the many-to-many relationship between Product and Ingredient
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship between Product and Ingredient
            base.OnModelCreating(modelBuilder);

            // composite key for ProductIngredient
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            // Each ProductIngredient belongs to one Product, and each Product can have many ProductIngredients
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            // Each ProductIngredient belongs to one Ingredient, and each Ingredient can be used in many ProductIngredients.
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);
            
            //seed Data for Categories
            modelBuilder.Entity<Category>().HasData
            (
                new Category { CategoryId = 1, Name = "Appetizers" },
                new Category { CategoryId = 2, Name = "Main Courses" },
                new Category { CategoryId = 3, Name = "Desserts" },
                new Category { CategoryId = 4, Name = "Beverages" }
            );
            //seed Data for Ingredients
            modelBuilder.Entity<Ingredient>().HasData
            (
                new Ingredient { IngredientId = 1, Name = "Tomato" },
                new Ingredient { IngredientId = 2, Name = "Lettuce" },
                new Ingredient { IngredientId = 3, Name = "Cheese" },
                new Ingredient { IngredientId = 4, Name = "Chicken" },
                new Ingredient { IngredientId = 5, Name = "Beef" }
            );
            //seed Data for Products
            modelBuilder.Entity<Product>().HasData(
                    new Product
                    { 
                      ProductId = 1, 
                      Name = "Beef Sandwich", 
                      Description = "Tomato, lettuce with Beef", 
                      Price = 8.99m, 
                      Stock = 50, 
                      CategoryId = 1
                    },
                    new Product 
                    { 
                        ProductId = 2, 
                        Name = "Grilled Chicken Sandwich", 
                        Description = "Juicy grilled chicken with lettuce and tomato", 
                        Price = 10.99m, 
                        Stock = 30, 
                        CategoryId = 2
                    },

                    new Product 
                    { 
                        ProductId = 3, 
                        Name = "Cheese Sandwich", 
                        Description = "Rich Cheese Sandwich with Tomato Slices", 
                        Price = 5.99m, 
                        Stock = 20, 
                        CategoryId = 3 
                    },
                    new Product 
                    { 
                        ProductId = 4, 
                        Name = "Coca-Cola", 
                        Description = "Refreshing soft drink", 
                        Price = 1.99m, 
                        Stock = 100, 
                        CategoryId = 4
                    }
                );

            //seed Data for ProductIngredients  
            modelBuilder.Entity<ProductIngredient>().HasData(
                
                new ProductIngredient { ProductId = 1, IngredientId = 1 }, // Tomato
                new ProductIngredient { ProductId = 1, IngredientId = 5 }, // beef
                new ProductIngredient { ProductId = 1, IngredientId = 2 }, // Lettuce

                new ProductIngredient { ProductId = 2, IngredientId = 1 }, // Tomato
                new ProductIngredient { ProductId = 2, IngredientId = 4 }, // chicken
                new ProductIngredient { ProductId = 2, IngredientId = 2 }, // Lettuce

                new ProductIngredient { ProductId = 3, IngredientId = 1 }, // Tomato
                new ProductIngredient { ProductId = 3, IngredientId = 2 }, // Lettuce
                new ProductIngredient { ProductId = 3, IngredientId = 3 }  // cheese
                );
        }
    }
}
