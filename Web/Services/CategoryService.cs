using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class CategoryService : GenericService<CategoryDTO, CategoryCreateDTO>, ICategoryService
    {
        private static string Url = "/api/Category";
        public CategoryService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(Url, _clientFactory, configuration)
        {
        }
    }
}
