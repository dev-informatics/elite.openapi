using System.Threading.Tasks;

namespace Eddi.Data.Core
{
    public interface IPersistenceRepository
    {
        Task SaveAllAsync(string fileName, string collectionName);

        Task DropTableAsync(string collectionName);
    }
}
