using Eddi.Models.Eddi.System.V1;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Eddi.LoaderService.ModelMaps.Eddi
{
    public class SystemV1Map : IDataStoreModelMap
    {
        public void Register()
        {
            BsonClassMap.RegisterClassMap<SystemV1>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}
