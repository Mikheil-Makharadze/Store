using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.DTO;

namespace Web.Models
{
    public class ProductSearchVM
    {
        public List<ProductDTO>? Products { get; set; }
        public SelectList? Categorys { get; set; }
        public string? category { get; set; }
        public string? SearchString { get; set; }

    }
}
