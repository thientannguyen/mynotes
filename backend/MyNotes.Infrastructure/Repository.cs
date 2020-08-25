using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNotes.Core.SharedKernel;

namespace MyNotes.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly IDomainEventPublisher _eventPublisher;
        private readonly LiteDbContext _dbContext;

        public Repository(IDomainEventPublisher eventPublisher, LiteDbContext dbContext)
        {
            _eventPublisher = eventPublisher;
            _dbContext = dbContext;
        }

        public Task<List<T>> ListAsync<T>(ISpecification<T> spec = null) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(string id) where T : AggregateRoot
        {
            return Task.Run(() => _dbContext.GetById<T>(id));
        }

        public Task<T> GetBySpecificationAsync<T>(ISpecification<T> spec) where T : AggregateRoot
        {
            return Task.Run(() => _dbContext.GetBySpec<T>(spec));
        }

        public async Task SaveAsync<T>(T entity) where T : AggregateRoot
        {
            _dbContext.UpsertItem(entity);
            await PublishEvents(entity);
        }

        private async Task PublishEvents(AggregateRoot entity)
        {
            var entityEvents = entity.Events.ToArray();
            entity.ClearEvents();
            await _eventPublisher.Publish(entityEvents);
        }
    }
}
