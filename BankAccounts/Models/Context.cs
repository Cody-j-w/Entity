using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> user{get; set;}
        public DbSet<Transaction> Transaction{get; set;}
    }
}