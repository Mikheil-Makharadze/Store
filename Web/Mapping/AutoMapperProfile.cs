using AutoMapper;
using Web.Models.DTO;

namespace Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Category

            CreateMap<CategoryDTO, CategoryUpdateDTO>()
                .ForPath(cd => cd.ProductsId, m => m.MapFrom(n => n.Products.Select(n => n.Id)))
                .ReverseMap();

            //Producer
            CreateMap<ProducerDTO, ProducerUpdateDTO>()
                .ForPath(cd => cd.ProductsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();

            //Product
            CreateMap<ProductDTO, ProductUpdateDTO>()
                .ForPath(pd => pd.ProducerId, m => m.MapFrom(p => p.Producer.Id))
                .ForPath(pd => pd.CategoriesId, m => m.MapFrom(p => p.Categories.Select(n => n.Id)))
                .ReverseMap();

        }
    }
}
