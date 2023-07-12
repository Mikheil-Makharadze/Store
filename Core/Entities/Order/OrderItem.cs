namespace Core.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }

        //Relationships
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
