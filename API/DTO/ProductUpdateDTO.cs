using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        //Relationships
        [Required]
        public int ProducerId { get; set; }
        [Required]
        public ICollection<int> CategoriesId { get; set; }
    }
}
