using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<int>? ProductsId { get; set; }

    }
}
