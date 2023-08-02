using Core.Entities.Identity;
using Core.Entities.Order;
using Core.Interfaces;
using Infrastructure.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext context;

        public OrderService(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = await context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Product).ToListAsync();

            return orders;
        }

        public async Task<List<Order>> GetOrdersByUserAsync(string userEmail)
        {
            var orders = await context.Orders.Where(n => n.UserEmail == userEmail).Include(n => n.OrderItems).ThenInclude(n => n.Product).ToListAsync();

            return orders;
        }

        public async Task<Order> StoreOrderAsync(List<OrderItem> items, User user)
        {
            double subtotal = 0.0;
            foreach (var item in items)
            {
                subtotal += item.Product.Price * item.Amount;
            }

            var order = new Order()
            {
                UserEmail = user.Email,
                Subtotal = subtotal
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    ProductId = item.ProductId,
                    OrderId = order.Id
                };

                await context.OrderItems.AddAsync(orderItem);
            }

            await context.SaveChangesAsync();

            return order;
        }
    }
}
