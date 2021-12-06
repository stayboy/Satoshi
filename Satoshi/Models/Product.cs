using Microsoft.EntityFrameworkCore;

namespace Satoshi.Models;

[Keyless]
public record ProductStat(string ProductName, int Sales)
{
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
    public DateTime? LastSold { get; set; }
}
public class Product : IdModel
{
    [MaxLength(120)]
    public string ProductName { get; set; }
    public float Price { get; set; }

    //[InverseProperty(nameof(Order.ProductId))]
    public virtual ICollection<Order> Orders { get; set; }
}
