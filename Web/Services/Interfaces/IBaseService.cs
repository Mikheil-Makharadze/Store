using Web.Models;

namespace Web.Services.Interfaces
{

    public interface IBaseService
    {
        Task<APIResponse> SendAsync(APIRequest apiRequest);
    }
}
