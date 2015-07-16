using Eddb.Sdk.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddb.Sdk.Data.MongoDb
{
    public class MongoDbCollectionRepository : ICollectionRepository
    {
        public MongoDbCollectionRepository(string connectionString, string database, string collection)
        {

        }

        public void BatchSaveAsync(IEnumerable<string> jsonCollection)
        {
            throw new NotImplementedException();
        }

        public void SaveAsync(string jsonString)
        {
            throw new NotImplementedException();
        }
    }
}
