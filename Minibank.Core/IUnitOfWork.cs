using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}