using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNotes.Core.SharedKernel
{
    public interface IRepository
    {
        Task<List<T>> ListAsync<T>(ISpecification<T> spec = null) where T : AggregateRoot;

        Task<T> GetByIdAsync<T>(string id) where T : AggregateRoot;

        Task<T> GetBySpecificationAsync<T>(ISpecification<T> spec) where T : AggregateRoot;

        Task SaveAsync<T>(T entity) where T : AggregateRoot;

    }
}
