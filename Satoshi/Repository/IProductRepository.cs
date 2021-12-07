using Satoshi.Enums;
using Satoshi.Models;

namespace Satoshi.Repository;
public interface IProductRepository : IRepository<Product>
{
    Task<Product> LoadProduct(int Id, int LoadTopOrders = 0, bool TrackChanges = false);
    IQueryable<Product> FindProducts(int[] Ids, string ProductName = null, float MinPrice = 0, float MaxPrice = 0, int SkipTotal = 0, int Top = 20, 
        ProductSort SortExpr = ProductSort.None);
    Task<IEnumerable<ProductStat>> FindProductStats(int? ProductId, string Term, DateTime? StartOrderDate, DateTime? EndOrderDate,
        float Price, float MaxPrice, int SkipTotal = 0, int Top = 20);
    Task<bool> ProductNameExists(string ProductName);
}
