namespace Satoshi.Models;
public class Order : IdModel
{
    public string CustomerName { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public float Price { get; set; }
}
