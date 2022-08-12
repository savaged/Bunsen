using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning.Testing.API
{
    public interface IDataService
    {
        Task<IEnumerable<T>> IndexAsync<T>() where T : IModel;

        Task<T> ShowAsync<T>(int id) where T : IModel;

        Task<T> StoreAsync<T>(T model) where T : notnull, IModel;

        Task UpdateAsync<T>(T model) where T : notnull, IModel;

        Task DeleteAsync<T>(int id) where T : IModel;
    }
}
