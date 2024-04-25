using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
    public interface IAgreegate<T> : IAgreegate, IEntity<T>
    {

    }
    public interface IAgreegate:IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }

        IDomainEvent[] ClearDomainEvents();
    }
}
