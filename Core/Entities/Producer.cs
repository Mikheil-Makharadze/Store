namespace Core.Entities
{
    public class Producer : BaseEntity
    {
        public Producer()
        {
             Products = new List<Product>();
        }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }

        //Relationships
        public List<Product> Products { get; set; }
    }
}
