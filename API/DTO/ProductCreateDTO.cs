using Core.Entities;

namespace API.DTO
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        //Relationships
        public int ProducerId { get; set; }
        public ICollection<int> CategoriesId { get; set; }
    }
}
