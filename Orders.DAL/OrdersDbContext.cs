using Microsoft.EntityFrameworkCore;
using Orders.DAL.Entity;

namespace Orders.DAL
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public OrdersDbContext(DbContextOptions options) : base(options) { }
    }
}