namespace Core.Entities
{
    public class Producer : BaseEntity
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }

        //Relationships
        public ICollection<Product> Products { get; set; }
    }
}
