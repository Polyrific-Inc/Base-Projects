using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var eventProcessor in EventProcessors)
            {
                if (eventProcessor is IEventProcessor<TEntity> eventHandler)
                {
                    await eventHandler.ProcessSaveEntity(this);
                }
            }
        }
    }
}
