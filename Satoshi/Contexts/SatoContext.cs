using Microsoft.EntityFrameworkCore;
using Satoshi.Models;

namespace Satoshi.Contexts;
public class SatoContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
