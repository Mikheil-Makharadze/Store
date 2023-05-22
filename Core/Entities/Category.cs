namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        //Relationships
        public ICollection<Product_Category> Product_Categories { get; set; }

    }
}
