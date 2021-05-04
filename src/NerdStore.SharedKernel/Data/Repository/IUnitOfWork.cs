using System.Threading.Tasks;

namespace NerdStore.SharedKernel.Data.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}