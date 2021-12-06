using Satoshi.Repository;

namespace Satoshi.Services
{
    public interface IOrderService
    {
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
