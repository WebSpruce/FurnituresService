using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurnituresServiceDatabase.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartFurniture> CartFurnitures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderFurniture> OrderFurnitures { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}