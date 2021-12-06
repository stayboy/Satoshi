using Satoshi.Contexts;
using Satoshi.Repository;

namespace Satoshi.Services;
public class OrderService : IOrderService
{
    private readonly SatoContext context;

    private OrderRepository orderRepo;
    private ProductRepository productRepo; 
    public OrderService(SatoContext _context)
    {
        context = _context;
    }
    public IOrderRepository OrderRepository
    {
        get
        {
            if (orderRepo == null) orderRepo = new OrderRepository(context);
            return orderRepo;
        }
    }

    public IProductRepository ProductRepository {
        get
        {
            if (productRepo == null) productRepo = new ProductRepository(context);
            return productRepo;
        }
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public Task SaveAsync()
    {
        return context.SaveChangesAsync();
    }
}
