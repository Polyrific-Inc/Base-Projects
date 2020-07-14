using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public interface IEventStorage
    {
        Task EmitEvent<TEntity>(BaseEvent<TEntity> eventData) where TEntity : BaseEntity;
    }
}
