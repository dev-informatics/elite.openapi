using System.Collections.Generic;

namespace Eddb.Sdk.Data.Core
{
    public interface ICollectionRepository
    {
        void SaveAsync(string jsonString);

        void BatchSaveAsync(IEnumerable<string> jsonCollection);
    }
}
