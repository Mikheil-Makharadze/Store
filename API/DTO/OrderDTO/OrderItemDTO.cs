using Core.Entities.Order;
using Core.Entities;

namespace API.DTO.OrderDTO
{
    public class OrderItemDTO
    {
        public int Amount { get; set; }

        //Relationships
        public ProductDTO Product { get; set; }
        public OrderDTO Order { get; set; }

    }
}
