namespace Eddb.Sdk.Data.Core
{
    public interface IPersistenceRepository
    {
        void Save<T>(T entity);
    }
}
