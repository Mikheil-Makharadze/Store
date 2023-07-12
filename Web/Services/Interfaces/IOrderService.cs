using Web.Models.DTO.OrderDTO;
namespace Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetUserOrder(string email, string token);
        Task<List<OrderDTO>> GetAllOrder(string? searchString, string token);
        Task CreateOrder(List<OrderItemDTO> orderItemsDTO, string email);
    }
}
