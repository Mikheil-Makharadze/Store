using Core.Entities.Identity;
using Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByUserAsync(string userEmail);
        Task<Order> StoreOrderAsync(List<OrderItem> items, User user);
    }
}
