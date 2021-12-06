using Satoshi.Enums;
using Satoshi.Models;

namespace Satoshi.Repository;
public interface IOrderRepository : IRepository<Order>
{
    Task<Order> LoadOrder(int Id, bool LoadProduct = true, bool TrackChanges = false);
    IQueryable<Order> FindOrders(int[] Ids, int Product = 0, string CustomerName = null, string ProductName = null,
        float MinPrice = 0, float MaxPrice = 0, int SkipTotal = 0, int Top = 10, OrderSort SortExpr = OrderSort.None);
    Task<bool> OrderExists(string Customer, int ProductId);
}
