namespace Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public Order()
        {
               OrderItems = new List<OrderItem>();  
        }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public double Subtotal { get; set; } = 0.0;

        //Relationships
        public string? UserEmail { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
