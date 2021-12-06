namespace Satoshi.Models;
public class Order : IdModel
{
    [MaxLength(150)]
    public string CustomerName { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    public float Price { get; set; }
}
