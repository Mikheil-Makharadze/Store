namespace Web.Models.DTO.OrderDTO
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; } 
        public double Subtotal { get; set; }
        public string UserEmail { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
