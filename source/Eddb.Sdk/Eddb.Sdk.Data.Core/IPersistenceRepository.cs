using System.Threading.Tasks;

namespace Eddb.Sdk.Data.Core
{
    public interface IPersistenceRepository
    {
        Task SaveAllAsync(string fileName, string collectionName);

        Task DropTableAsync(string collectionName);
    }
}
