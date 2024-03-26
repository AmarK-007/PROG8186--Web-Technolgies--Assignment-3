using Microsoft.EntityFrameworkCore;
using Shoecart_ASP_Assignment3.Models;

namespace Shoecart_ASP_Assignment3.Data {
    public class MyDBContext : DbContext // Inherit from DbContext
    {
        // Constructor
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {
        }
        //public DbSet<WEBAssignment3.Models.User> User { get; set; } = default!;
        public DbSet<User> User { get; set; }

    }
}
