using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        //Relationships
        public ICollection<int>? productsId { get; set; }

    }
}
