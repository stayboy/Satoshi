namespace Satoshi.DTO;
public class OrderDTO
{
    public string CustomerName { get; set; }
    public int ProductId { get; set; }
    public ProductDTO Product { get; set; }
    public float Price { get; set; }
}
