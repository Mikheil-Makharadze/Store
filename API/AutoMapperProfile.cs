using API.DTO;
using API.DTO.IdentityDTO;
using API.DTO.OrderDTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Order;

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

            //Order
            CreateMap<Order, OrderDTO>()
                .ForMember(pd => pd.UserEmail, m => m.MapFrom(p => p.User.Email))
                .ForMember(pd => pd.OrderDate, m => m.MapFrom(p => p.OrderDate))
                .ForMember(pd => pd.Subtotal, m => m.MapFrom(p => p.Subtotal))
                .ForMember(pd => pd.OrderItems, m => m.MapFrom(p => p.OrderItems));

            //OrderItem
            CreateMap<OrderItem, OrderItemDTO>()
                .ForPath(pd => pd.Price, m => m.MapFrom(p => p.Price))
                .ForPath(pd => pd.Amount, m => m.MapFrom(p => p.Amount))
                .ForPath(pd => pd.ProductId, m => m.MapFrom(p => p.ProductId))
                .ReverseMap();
        }
    }
}
