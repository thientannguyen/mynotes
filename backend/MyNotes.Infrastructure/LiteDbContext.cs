using System;
using System.Collections.Generic;
using System.Linq;
using MyNotes.Core.SharedKernel;
using LiteDB;

namespace MyNotes.Infrastructure
{
    public abstract class LiteDbContext : IDisposable
    {
        public abstract string ContainerName { get; }

        protected LiteDbContext(ConnectionString config)
        {
            var db = new LiteDatabase(config);
            Database = db;
        }

        protected abstract void CreateModel();

        public ILiteDatabase Database { get; }

        public void Initialize()
        {
            CreateModel();
        }

        public virtual T GetById<T>(string id) where T : AggregateRoot
        {
            using (Database)
            {
                Database.BeginTrans();
                var col = Database.GetCollection<T>(ContainerName);
                var result = col.FindById(id);
                Database.Commit();
                return result;
            }
        }

        public virtual T GetBySpec<T>(ISpecification<T> spec) where T : AggregateRoot
        {
            using (Database)
            {
                Database.BeginTrans();
                var col = Database.GetCollection<T>(ContainerName);
                var result = col.Find(spec.Criteria);
                Database.Commit();
                return GetFirstOrDefaultFromEnumerable(result);
            }
        }

        public virtual bool UpsertItem<T>(T item) where T : AggregateRoot
        {
            using (Database)
            {
                Database.BeginTrans();
                var col = Database.GetCollection<T>(ContainerName);
                var result = col.Upsert(item.id, item);
                Database.Commit();
                return result;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Database?.Dispose();
        }

        private T GetFirstOrDefaultFromEnumerable<T>(IEnumerable<T> iterator) where T : AggregateRoot
        {
            IEnumerable<T> aggregateRoots = iterator.ToList();
            return !aggregateRoots.Any() ? null : aggregateRoots.FirstOrDefault();
        }
    }
}
