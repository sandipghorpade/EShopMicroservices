﻿using MediatR;

namespace Ordering.Infastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator):SaveChangesInterceptor
    {       
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var aggregates = context.ChangeTracker
                   .Entries<IAgreegate>()
                   .Where(a => a.Entity.DomainEvents.Any())
                   .Select(a => a.Entity);

            var domainEvents = aggregates
                               .SelectMany(a=>a.DomainEvents)
                               .ToList();

            aggregates.ToList().ForEach(a => { a.ClearDomainEvents(); });

            foreach ( var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
