namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
                Product_Categories = new List<Product_Category>();
        }
        public string? Name { get; set; }

        //Relationships
        public List<Product_Category> Product_Categories { get; set; }

    }
}
