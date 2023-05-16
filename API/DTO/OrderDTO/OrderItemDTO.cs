using Core.Entities.Order;
using Core.Entities;

namespace API.DTO.OrderDTO
{
    public class OrderItemDTO
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
    }
}
