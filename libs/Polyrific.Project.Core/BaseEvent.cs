using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public abstract class BaseEvent
    {
        public BaseEvent()
        {
            EventId = Guid.NewGuid();
        }

        public Guid EventId { get; set; }

        public abstract Task Process();

        public abstract void SetEventProcessor<TBaseEntity>(IEventProcessor<TBaseEntity> eventProcessor) where TBaseEntity : BaseEntity;
    }

    public abstract class BaseEvent<TEntity> : BaseEvent where TEntity : BaseEntity
    {
        public IEventProcessor<TEntity> EventProcessor { get; set; }

        public override void SetEventProcessor<TBaseEntity>(IEventProcessor<TBaseEntity> eventProcessor)
        {
            EventProcessor = eventProcessor as IEventProcessor<TEntity>;
        }
    }
}
