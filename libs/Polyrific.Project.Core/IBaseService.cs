using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    /// <summary>
    /// The base interface of entity service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>The <see>Result</see> of the operation</returns>
        Task<Result> Delete(int id);

        /// <summary>
        /// Get and entity by its Id
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>The entity</returns>
        Task<TEntity> Get(int id);

        /// <summary>
        /// Get a collection of entities along with paging properties
        /// </summary>
        /// <param name="page">Current page number (starts from 1)</param>
        /// <param name="pageSize">The maximum number of entities in a page</param>
        /// <param name="orderBy">The name of the property for the result to be ordered</param>
        /// <param name="filter">The name and value of the filters in the format of "{PropertyName1}_{PropertyValue1},{PropertyName2}_{PropertyValue2}"</param>
        /// <param name="descending">Sort the result in descending order?</param>
        /// <returns>The paging object</returns>
        Task<Paging<TEntity>> GetPageData(int? page = null,
            int? pageSize = null,
            string orderBy = null,
            string filter = null,
            bool @descending = false);

        /// <summary>
        /// Save an entity (either creating new entity or updating an existing one)
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <param name="createIfNotExist">Create new entity if it doesn't exist (default = false)</param>
        /// <param name="userEmail">Email of current user (to be put in <c>CreatedBy</c> and <c>UpdatedBy</c>)</param>
        /// <param name="userDisplayName">Display Name of current user (to be put in <c>CreatedBy</c> and <c>UpdatedBy</c>)</param>
        /// <returns>The <see>Result<T></see> of the operation</returns>
        Task<Result<TEntity>> Save(TEntity entity, bool createIfNotExist = false, string userEmail = null, string userDisplayName = null);
    }
}
