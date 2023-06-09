using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class ProductUpdateDTO
    {
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
        [DisplayName("Producer")]

        public int ProducerId { get; set; }
        [Required]
        [DisplayName("Categories")]
        public ICollection<int> CategoriesId { get; set; }
    }
}
