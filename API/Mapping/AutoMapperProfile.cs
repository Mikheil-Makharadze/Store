using API.DTO;
using API.DTO.IdentityDTO;
using API.DTO.OrderDTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Order;

namespace API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Category, CategoryCreateDTO>().ReverseMap();

            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Product, SelectorOption>()
                .ForPath(SO => SO.Id, p => p.MapFrom(c => c.Id))
                .ForPath(SO => SO.Name, p => p.MapFrom(c => c.Name));

            //Producer
            CreateMap<Producer, ProducerDTO>().ReverseMap();

            CreateMap<Producer, ProducerCreateDTO>()
                .ForPath(cd => cd.ProductsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();
            CreateMap<Producer, ProducerUpdateDTO>()
                .ForPath(cd => cd.ProductsId, m => m.MapFrom(c => c.Products.Select(n => n.Id)))
                .ReverseMap();

            CreateMap<Producer, SelectorOption>()
                .ForPath(SO => SO.Id,  p => p.MapFrom(c => c.Id))
                .ForPath(SO => SO.Name, p => p.MapFrom(c => c.Name));


            //Product
            CreateMap<Product, ProductDTO>()
                .ForPath(pd => pd.Producer, m => m.MapFrom(p => p.Producer))
                .ReverseMap();

            CreateMap<Product, ProductCreateDTO>()
                .ForPath(pd => pd.ProducerId, m => m.MapFrom(p => p.ProducerId))
                .ReverseMap();
            CreateMap<Product, ProductUpdateDTO>()
                .ForPath(pd => pd.ProducerId, m => m.MapFrom(p => p.ProducerId))
                .ReverseMap();

            CreateMap<Product, SelectorOption>()
                .ForPath(SO => SO.Id, p => p.MapFrom(c => c.Id))
                .ForPath(SO => SO.Name, p => p.MapFrom(c => c.Name));

            //Order
            CreateMap<Order, OrderDTO>()
                .ForPath(pd => pd.Status, m => m.MapFrom(p => p.Status))
                .ForMember(pd => pd.OrderDate, m => m.MapFrom(p => p.OrderDate))
                .ForMember(pd => pd.Subtotal, m => m.MapFrom(p => p.Subtotal))
                .ForMember(pd => pd.OrderItems, m => m.MapFrom(p => p.OrderItems));

            //OrderItem
            CreateMap<OrderItem, OrderItemDTO>()
                .ForPath(pd => pd.Amount, m => m.MapFrom(p => p.Amount))
                .ForPath(pd => pd.Product, m => m.MapFrom(p => p.Product))
                .ReverseMap();
        }
    }
}
