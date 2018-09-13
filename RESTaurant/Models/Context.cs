using Microsoft.EntityFrameworkCore;

namespace RESTaurant.Models
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options) : base(options) { }

        public DbSet<Restaur> Restaurants{get; set;}
    }
}