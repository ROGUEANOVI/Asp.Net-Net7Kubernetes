using Net7Kubernetes.Models;

namespace Net7Kubernetes.Data.interfaces
{
    public interface IEstateRepository
    {
        Task<IEnumerable<Estate>> GetAllEstates();
        
        Task<Estate> GetEstateById(int id);

        Task CreateEstate(Estate estate);

        Task DeleteEstate(int id);

        Task<bool> SaveChanges();

    }
}