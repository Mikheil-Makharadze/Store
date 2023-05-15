using API.DTO;
using API.DTO.IdentityDTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Category
            CreateMap<Category, CategoryDTO>()
                .ForPath(cd => cd.productsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap(); 
            CreateMap<Category, CategoryCreateDTO>()
                .ForPath(cd => cd.productsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();

            //Producer
            CreateMap<Producer, ProducerDTO>()
                .ForPath(cd => cd.productsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();
            CreateMap<Producer, ProducerCreateDTO>()
                .ForPath(cd => cd.productsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();


            //Product
            CreateMap<Product, ProductDTO>()
                .ForPath(pd => pd.ProducerId, m => m.MapFrom(p => p.ProducerId))
                .ForPath(pd => pd.CategoriesId, m => m.MapFrom(p => p.Categories.Select(n => n.Id)))
                .ReverseMap();
            CreateMap<Product, ProductCreateDTO>()
                .ForPath(pd => pd.ProducerId, m => m.MapFrom(p => p.ProducerId))
                .ForPath(pd => pd.CategoriesId, m => m.MapFrom(p => p.Categories.Select(n => n.Id)))
                .ReverseMap();
        }
    }
}
