using Core.Entities;

namespace Core.Interfaces
{
    public interface IProducerService : IGenericRepository<Producer>
    {
        Task<IEnumerable<Producer>> GetAllProducersDetails(string? search); 
    }
}
