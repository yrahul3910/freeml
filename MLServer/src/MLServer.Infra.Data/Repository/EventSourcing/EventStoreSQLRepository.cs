using System;
using System.Collections.Generic;
using System.Linq;
using MLServer.Domain.Core.Events;
using MLServer.Infra.Data.Context;

namespace MLServer.Infra.Data.Repository.EventSourcing
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly EventStoreSqlContext _context;

        public EventStoreSqlRepository(EventStoreSqlContext context)
        {
            _context = context;
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvents.Add(theEvent);
            _context.SaveChanges();
        }

        public IList<StoredEvent> All(Guid aggregateId)
        {
            return (from e in _context.StoredEvents where e.AggregateId == aggregateId select e).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}