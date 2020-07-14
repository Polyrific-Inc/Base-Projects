using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public interface IEventProcessor<TEntity> where TEntity : BaseEntity
    {
        Task ProcessSaveEntity(SaveEntityEvent<TEntity> eventData);
        Task ProcessDeleteEntity(DeleteEntityEvent<TEntity> eventData);
    }
}
