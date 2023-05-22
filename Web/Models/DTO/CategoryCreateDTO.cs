namespace Web.Models.DTO
{
    public class CategoryCreateDTO
    {
        public string Name { get; set; }

        //Relationships
        public ICollection<int>? productsId { get; set; }
    }
}
