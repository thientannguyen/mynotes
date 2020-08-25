using System;
using System.Linq.Expressions;

namespace MyNotes.Core.SharedKernel
{
    public interface ISpecification<T> where T : AggregateRoot
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
