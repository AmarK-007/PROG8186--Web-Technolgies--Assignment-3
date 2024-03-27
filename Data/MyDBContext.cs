using Microsoft.EntityFrameworkCore;
using Shoecart_ASP_Assignment3.Models;
using System;
using System.Data;

namespace Shoecart_ASP_Assignment3.Data {
    public class MyDBContext : DbContext {
        // Constructor
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {
        }


        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .Property(p => p.ShippingCost)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
