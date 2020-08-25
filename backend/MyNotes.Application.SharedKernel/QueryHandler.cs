namespace MyNotes.Application.SharedKernel
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class QueryHandler<TQuery, TQueryResult>
        : IRequestHandler<TQuery, TQueryResult> where TQuery : Query<TQueryResult>
    {
        public Task<TQueryResult> Handle(TQuery query)
            => Handle(query, CancellationToken.None);

        public Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken)
            => ExecuteQuery(request);

        protected abstract Task<TQueryResult> ExecuteQuery(TQuery query);
    }
}
