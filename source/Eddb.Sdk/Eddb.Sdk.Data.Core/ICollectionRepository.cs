using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eddb.Sdk.Data.Core
{
    public interface ICollectionRepository
    {
        Task SaveAsync(string jsonString);

        Task BatchSaveAsync(IEnumerable<string> jsonCollection);

        Task DropCollectionAsync();
    }
}
