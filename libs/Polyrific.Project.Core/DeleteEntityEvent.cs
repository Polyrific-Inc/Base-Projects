using System;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public class DeleteEntityEvent<TEntity> : BaseEvent<TEntity> where TEntity: BaseEntity
    {
        public DeleteEntityEvent(int id, string additionalKey = null)
        {
            EntityId = id;
            AdditionalKey = additionalKey;
        }

        public int EntityId { get; set; }

        public string AdditionalKey { get; set; }

        public async override Task Process()
        {
            if (EventProcessor is IEventProcessor<TEntity> eventHandler)
            {
                await eventHandler.ProcessDeleteEntity(this);
            }
        }
    }
}
