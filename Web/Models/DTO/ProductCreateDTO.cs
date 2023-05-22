namespace Web.Models.DTO
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        //Relationships
        public int ProducerId { get; set; }
        public ICollection<int> CategoriesId { get; set; }
    }
}
