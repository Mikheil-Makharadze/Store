using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }

        //Relationships
        public ICollection<int>? ProductsId { get; set; }
    }
}
