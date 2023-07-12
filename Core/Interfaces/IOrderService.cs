using Core.Entities.Identity;
using Core.Entities.Order;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByUserAsync(string userEmail);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> StoreOrderAsync(List<OrderItem> items, User user);
    }
}
