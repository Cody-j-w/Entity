using Microsoft.EntityFrameworkCore;

namespace ProductManager.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Product> Products{get; set;}
        public DbSet<Category> Categories{get; set;}

        public DbSet<ProductCategory> ProductCategory{get; set;}
    }
}