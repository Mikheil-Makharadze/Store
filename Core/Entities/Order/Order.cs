namespace Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public double Subtotal { get; set; }

        //Relationships
        public string UserEmail { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
