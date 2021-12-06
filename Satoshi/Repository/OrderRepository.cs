using Microsoft.EntityFrameworkCore;
using Satoshi.Contexts;
using Satoshi.Enums;
using Satoshi.Models;
using System.Linq.Expressions;

namespace Satoshi.Repository;
public class OrderRepository : DBRepositoryBase<Order, SatoContext>, IOrderRepository
{
    public OrderRepository(SatoContext _context) : base(_context)
    {
    }
    public IQueryable<Order> FindOrders(int[] Ids, int Product, string CustomerName, string ProductName, float MinPrice, 
        float MaxPrice, int SkipTotal, int Top, OrderSort SortExpr)
    {
        var rs = Entity.Where(x => (!Ids.Any() || Ids.Any(o => x.Id == o)) &&
            (Product == 0 || x.ProductId == Product) &&
            (string.IsNullOrWhiteSpace(CustomerName) || x.CustomerName.Contains(CustomerName)) &&
            (string.IsNullOrWhiteSpace(ProductName) || x.Product.ProductName.Contains(ProductName)) &&
            (MinPrice == 0 || MaxPrice == 0 || (
                (MaxPrice == 0 && x.Price >= MinPrice) ||
                (x.Price >= MinPrice && x.Price <= MaxPrice)
            )));

        rs = rs.Include(x => x.Product);
        rs = SortExpr switch
        {
           OrderSort.Customer_Name => rs.OrderBy(x => x.CustomerName),
           OrderSort.Product_Name => rs.OrderBy(x => x.Product.ProductName),
           OrderSort.Customer_Name_Desc => rs.OrderByDescending(x => x.CustomerName),
           OrderSort.Product_Name_Desc => rs.OrderByDescending(x => x.Product.ProductName),
           OrderSort.Price => rs.OrderBy(x => x.Price),
           _ => rs.OrderByDescending(x => x.Price)
        };
        if (SkipTotal > 0) rs = rs.Skip(SkipTotal);
        rs = rs.Take(Top);

        return rs;
    }

    public Task<Order> LoadOrder(int Id, bool LoadProduct, bool TrackChanges)
    {
        var rs = Entity.Where(x => x.Id == Id);
        rs = rs.Include(x => x.Product);

        if (!TrackChanges) rs = rs.AsNoTracking();
        return rs.SingleOrDefaultAsync();
    }

    public Task<bool> OrderExists(string Customer, int ProductId)
    {
        return Entity.AnyAsync(o => string.Equals(Customer, o.CustomerName) && o.ProductId == ProductId);
    }

    void IRepository<Order>.CreateEntity(Order entity)
    {
        Create(entity);
    }

    void IRepository<Order>.DeleteEntity(Order entity)
    {
        Delete(entity);
    }

    void IRepository<Order>.UpdateEntity(Order entity)
    {
        Update(entity);
    }
}
