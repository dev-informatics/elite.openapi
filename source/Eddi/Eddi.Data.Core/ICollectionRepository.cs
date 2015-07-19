using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eddi.Data.Core
{
    public interface ICollectionRepository
    {
        Task SaveAsync(string jsonString);

        Task BatchSaveAsync(IEnumerable<string> jsonCollection);

        Task DropCollectionAsync();
    }

    public interface ICollectionRepository<T>
    {
        Task SaveAsync(T item);

        Task BatchSaveAsync(IEnumerable<T> items);

        Task DropCollectionAsync();

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filterExpression);

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>> sortExpression, int sortDirection = 1);
    }
}
