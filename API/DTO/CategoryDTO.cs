using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relationships
        public ICollection<ProductDTO>? Products { get; set; }

    }
}
