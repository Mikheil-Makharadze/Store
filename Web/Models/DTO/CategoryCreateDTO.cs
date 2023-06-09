using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; }

        //Relationships
        [DisplayName("Games")]
        public ICollection<int>? ProductsId { get; set; }
    }
}
