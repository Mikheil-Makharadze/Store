namespace Web.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        //Relationships
        public ProducerDTO Producer { get; set; }
        public ICollection<CategoryDTO> Categories { get; set; }
    }
}
