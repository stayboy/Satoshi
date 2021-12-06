namespace Satoshi.DTO;
public class ProductDTO
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public float Price { get; set; }     
    public IList<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
}
