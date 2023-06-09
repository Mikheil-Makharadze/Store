using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayName("Games")]
        public ICollection<int>? ProductsId { get; set; }

    }
}
