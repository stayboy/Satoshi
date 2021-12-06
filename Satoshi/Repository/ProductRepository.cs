using Microsoft.EntityFrameworkCore;
using Satoshi.Contexts;
using Satoshi.Enums;
using Satoshi.Models;

namespace Satoshi.Repository;
public class ProductRepository : DBRepositoryBase<Product, SatoContext>, IProductRepository
{
    public ProductRepository(SatoContext _context) : base(_context)
    {
    }

    public void CreateEntity(Product entity)
    {
        Create(entity);
    }

    public void DeleteEntity(Product entity)
    {
        Delete(entity);
    }

    public IQueryable<Product> FindProducts(int[] Ids, string ProductName, float MinPrice, float MaxPrice, int SkipTotal, int Top, 
        ProductSort SortExpr)
    {
        var rs = Entity.Where(x => (!Ids.Any() || Ids.Any(o => o == x.Id)) &&
            (string.IsNullOrWhiteSpace(ProductName) || x.ProductName.Contains(ProductName)) &&
            (MinPrice == 0 || MaxPrice == 0 || (
                (MaxPrice == 0 && x.Price >= MinPrice) ||
                (x.Price >= MinPrice && x.Price <= MaxPrice)
            )));
        rs = SortExpr switch
        {
            ProductSort.Price => rs.OrderBy(x => x.Price),
            ProductSort.Price_Desc => rs.OrderByDescending(x => x.Price),
            ProductSort.ProductName_Desc => rs.OrderByDescending(x => x.ProductName),
            _ => rs.OrderBy(x => x.ProductName)
        };
        if (SkipTotal > 0) rs = rs.Skip(SkipTotal);
        rs = rs.Take(Top);

        return rs;
    }

    public async Task<IEnumerable<ProductStat>> FindProductStats(int? ProductId, string Term, DateTime? StartOrderDate, DateTime? EndOrderDate)
    {
        var rs = Entity.Where(x => (ProductId == 0 || x.Id == ProductId) &&
             (string.IsNullOrWhiteSpace(Term) || x.ProductName.Contains(Term)) &&
             (!StartOrderDate.HasValue || !EndOrderDate.HasValue || (
                 (!EndOrderDate.HasValue && x.DateCreated >= StartOrderDate) ||
                 (x.DateCreated >= StartOrderDate && x.DateCreated <= EndOrderDate)
             )));

        return await rs.Select(x => new ProductStat(x.ProductName, x.Orders.Count)
        {
            LastSold = x.Orders.Max(x => x.DateCreated),
            MinPrice = x.Orders.Min(x => x.Price),
            MaxPrice = x.Orders.Max(x => x.Price)
        }).ToListAsync();            
    }

    public Task<Product> LoadProduct(int Id, int LoadTopOrders, bool TrackChanges)
    {
        var rs = Entity.Where(x => x.Id == Id);
        if (LoadTopOrders > 0) rs = rs.Include(x => x.Orders.OrderByDescending(x => x.Price).Take(LoadTopOrders));
        if (!TrackChanges) rs = rs.AsNoTracking();

        return rs.SingleOrDefaultAsync();
    }

    public Task<bool> ProductNameExists(string ProductName)
    {
        return Entity.AnyAsync(o => string.Equals(ProductName, o.ProductName));
    }

    public void UpdateEntity(Product entity)
    {
        Update(entity);
    }
}
