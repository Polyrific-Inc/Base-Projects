using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public class SaveEntityEvent<TEntity> : BaseEvent<TEntity> where TEntity : BaseEntity
    {
        public SaveEntityEvent(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; set; }

        public async override Task Process()
        {
            if (EventProcessor is IEventProcessor<TEntity> eventHandler)
            {
                await eventHandler.ProcessSaveEntity(this);
            }
        }
    }
}
