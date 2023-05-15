namespace API.DTO
{
    public class ProducerCreateDTO
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }

        //Relationships
        public ICollection<int>? productsId { get; set; }

    }
}
