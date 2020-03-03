using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public interface IBaseService<T> where T : BaseEntity<T>
    {
        Task<Result> Delete(int id);

        Task<T> Get(int id);

        Task<Paging<T>> GetPageData(int page, int pageSize);

        Task<Result<T>> Save(T entity, bool createIfNotExist = false);
    }
}
