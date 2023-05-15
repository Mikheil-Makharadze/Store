namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        //Relationships
        public ICollection<Product> Products { get; set; }

    }
}
