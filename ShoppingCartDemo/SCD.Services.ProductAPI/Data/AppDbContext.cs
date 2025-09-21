using Microsoft.EntityFrameworkCore;
using SCD.Services.ProductAPI.Models;

namespace SCD.Services.ProductAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product { ID = 1, Name = "T-shirt", Category = "Clothing", Price = 500, Description = "Nice t-shirt", ImageUrl = "/images/110040ba-d84e-48bc-b12a-3953a8c147b4.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 2, Name = "Jeans", Category = "Clothing", Price = 1100, Description = "Chipest jeans", ImageUrl = "/images/404d8e91-5765-4ae6-ae8d-5f9f9c4d087e.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 3, Name = "Jacket", Category = "Clothing", Price = 3500, Description = "Stylist jacket", ImageUrl = "/images/872eb7f6-3f47-46b1-baa1-96e3bf567b78.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 4, Name = "Shoes", Category = "Footwear", Price = 800, Description = "Cool shoes", ImageUrl = "/images/e1e83066-802d-40b8-bdbc-dacbc63a848b.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 5, Name = "Sandals", Category = "Footwear", Price = 600, Description = "Princes style", ImageUrl = "/images/98bb4ebf-3794-4a3e-a2d0-4ef4e0380f9a.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 6, Name = "Cap", Category = "Accessories", Price = 150, Description = "Looks rocking ", ImageUrl = "/images/cbee0ecf-19dc-400a-892f-35122d43b00f.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 7, Name = "Watch", Category = "Accessories", Price = 2000, Description = "Awesome watch", ImageUrl = "/images/bed1a22a-4147-4546-8aab-06301a3df2cd.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 8, Name = "Laptop Bag", Category = "Accessories", Price = 1800, Description = "Compact bag", ImageUrl = "/images/3202cac7-8d3c-4d93-8235-a6fd7f1a85ed.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 9, Name = "Headphones", Category = "Electronics", Price = 600, Description = "High base", ImageUrl = "/images/b4c2f781-04f3-4add-aabf-89943f03ee07.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 10, Name = "Laptop", Category = "Electronics", Price = 35000, Description = "Huge features", ImageUrl = "/images/b3cddf57-7051-43b5-b8f7-915f9913b30e.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 11, Name = "Lofer", Category = "Footwear", Price = 1500, Description = "Cool lofer", ImageUrl = "/images/b3cddf57-7051-43b5-b8f7-915f9913b30e.png" });
            modelBuilder.Entity<Product>().HasData(new Product { ID = 12, Name = "Induction", Category = "Electronics", Price = 2500, Description = "Smart induction", ImageUrl = "/images/442ad0e3-ddd2-474d-8877-660bc7114f0f.png" });
        }
    }
}
