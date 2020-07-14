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

        public abstract void AddEventProcessor<TBaseEntity>(IEventProcessor<TBaseEntity> eventProcessor) where TBaseEntity : BaseEntity;
    }

    public abstract class BaseEvent<TEntity> : BaseEvent where TEntity : BaseEntity
    {
        public BaseEvent()
        {
            EventProcessors = new List<IEventProcessor>();
        }

        public List<IEventProcessor> EventProcessors { get; set; }

        public override void AddEventProcessor<TBaseEntity>(IEventProcessor<TBaseEntity> eventProcessor)
        {
            EventProcessors.Add(eventProcessor as IEventProcessor);
        }
    }
}
