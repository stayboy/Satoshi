using Microsoft.EntityFrameworkCore;
using Satoshi.Models;

namespace Satoshi.Contexts;
public class SatoContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public SatoContext()
    {

    }
    public SatoContext(DbContextOptions<SatoContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new Product[]
        {
            new Product { Id = 1, ProductName = "Laptop", Price = 900},
            new Product { Id = 2, ProductName = "Keyboard", Price = 35},
            new Product{ Id = 3, ProductName = "Paper", Price = 5 }
        });
        modelBuilder.Entity<Order>().HasData(new Order[]
        {
            new Order { Id = 1, ProductId = 1, CustomerName = "Dave", Price = 900 },
            new Order { Id = 2, CustomerName = "George", ProductId = 2, Price = 35},
            new Order { Id = 3, CustomerName = "Fiona", ProductId = 3, Price = 5},
            new Order { Id = 4, CustomerName = "Rory", ProductId = 3, Price=3},
            new Order { Id = 5, CustomerName = "Olivia", ProductId = 1, Price = 600}
        });
        /*
         * {id :1, customer: Dave, product: Laptop, price:900}, 
         * {id:2, customer: George, product: keyboard, price:35} 
         * {id :3, customer: Fiona, product: paper, price:5}
         * {id :4, customer: Rory, product: paper, price:3}
         * {id :5, customer: Olivia, product: laptop, price:600}
         */

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=DbLite/Satoshi.db;Command Timeout=60");
        }
        base.OnConfiguring(optionsBuilder);
    }
}
