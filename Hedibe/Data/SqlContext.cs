using Hedibe.Models;
using Microsoft.EntityFrameworkCore;

namespace Hedibe.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<ShoppingList> ShoppingLists {get; set;}
   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(r => r.Username)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
               .Property(r => r.Name)
               .IsRequired();

            modelBuilder.Entity<Meal>()
                .Property(r => r.Name)
                .IsRequired();
        }
    }
}
