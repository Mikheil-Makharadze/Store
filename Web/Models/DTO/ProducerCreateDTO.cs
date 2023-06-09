using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class ProducerCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Logo { get; set; }
        [Required]
        public string Description { get; set; }

        //Relationships
        [DisplayName("Games")]
        public ICollection<int>? ProductsId { get; set; }

    }
}
