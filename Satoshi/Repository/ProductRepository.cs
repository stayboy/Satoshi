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
        IQueryable<Product> rs = null;
        if (Ids?.Any() ?? false) rs = Entity.Where(x => Ids.Any(o => o == x.Id));
        else rs = Entity.Where(x => 
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

    public async Task<IEnumerable<ProductStat>> FindProductStats(int? ProductId, string Term, DateTime? StartOrderDate, DateTime? EndOrderDate,
        float MinPrice, float MaxPrice, int SkipTotal, int Top)
    {
        /*var rs = Entity.Where(x => (ProductId == 0 || x.Id == ProductId) &&
             (string.IsNullOrWhiteSpace(Term) || x.ProductName.Contains(Term)) &&
             (!StartOrderDate.HasValue || !EndOrderDate.HasValue || (
                 (!EndOrderDate.HasValue && x.DateCreated >= StartOrderDate) ||
                 (x.DateCreated >= StartOrderDate && x.DateCreated <= EndOrderDate)
             )));
        */
        var q = from s in Entity
                join d in pcontext.Orders on s.Id equals d.ProductId into ogroup
                from o in ogroup.DefaultIfEmpty()
                where ((ProductId ?? 0) == 0 || s.Id == ProductId) &&
                    (string.IsNullOrWhiteSpace(Term) || s.ProductName.Contains(Term)) &&
                      (!StartOrderDate.HasValue || !EndOrderDate.HasValue || (
                         (!EndOrderDate.HasValue && s.DateCreated >= StartOrderDate) ||
                         (s.DateCreated >= StartOrderDate && s.DateCreated <= EndOrderDate)
                     )) && (MinPrice == 0 || MaxPrice == 0 || (
                    (MaxPrice == 0 && s.Price >= MinPrice) ||
                    (s.Price >= MinPrice && s.Price <= MaxPrice)))
                group new { o.Price, o.DateCreated, o.CustomerName, o.Id } by new { s.Id, s.ProductName } into mgroup
                select new ProductStat(mgroup.Key.Id, mgroup.Key.ProductName, mgroup.Select(x => x.Id).Count())
                {
                    LastSold = mgroup.Max(q => q.DateCreated),
                    MinPrice = mgroup.Min(q => q.Price),
                    MaxPrice = mgroup.Max(q => q.Price)
                };

        if (SkipTotal > 0) return await q.Skip(SkipTotal).Take(Top).ToListAsync();
        return await q.Take(Top).ToListAsync();
        //q.ToQueryString();
        /*
        return await rs.Select(x => new ProductStat(x.Id, x.ProductName, x.Orders.Count)
        {
            LastSold = x.Orders.Max(x => x.DateCreated),
            MinPrice = x.Orders.Min(x => x.Price),
            MaxPrice = x.Orders.Max(x => x.Price)
        }).ToListAsync();   
        */
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
