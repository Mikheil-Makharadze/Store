using Core.Entities.Order;

namespace API.DTO.OrderDTO
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public double Subtotal { get; set; }

        //RelationShip
        public string UserEmail { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
