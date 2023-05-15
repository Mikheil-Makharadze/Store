namespace API.DTO
{
    public class CategoryCreateDTO
    {
        public string CategoryName { get; set; }

        //Relationships
        public ICollection<int>? productsId { get; set; }
    }
}
