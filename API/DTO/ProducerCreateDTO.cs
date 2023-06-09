using System.ComponentModel.DataAnnotations;

namespace API.DTO
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
        public ICollection<int>? ProductsId { get; set; }

    }
}
