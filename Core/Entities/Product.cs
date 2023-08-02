namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Product_Categories = new List<Product_Category>();
        }

        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; } = 0;

        //Relationships
        public int? ProducerId { get; set; }
        public Producer? Producer { get; set; }
        public ICollection<Product_Category> Product_Categories { get; set; }

    }
}
