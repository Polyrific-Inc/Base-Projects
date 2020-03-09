using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    /// <inheritdoc/>
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        private readonly string _entityTypeName;

        /// <summary>
        /// Initiate this service
        /// </summary>
        /// <param name="repository">The main repository of this service</param>
        /// <param name="logger">Logger object to perform logging</param>
        protected BaseService(IRepository<TEntity> repository, ILogger logger)
        {
            Logger = logger;
            Repository = repository;
            _entityTypeName = typeof(TEntity).Name;
        }

        /// <summary>
        /// The main repository of this service
        /// </summary>
        protected IRepository<TEntity> Repository { get; }

        /// <summary>
        /// Logger object to perform logging
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc/>
        public virtual async Task<Result> Delete(int id)
        {
            try
            {
                await Repository.Delete(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete {_entityTypeName} {id}", _entityTypeName, id);

                return Result.FailedResult($"Failed to delete {_entityTypeName} {id}. Please check the logs.");
            }

            Logger.LogInformation("{_entityTypeName} {webClientId} has been deleted successfully", _entityTypeName, id);

            return Result.SuccessResult;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> Get(int id)
        {
            try
            {
                return await Repository.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get {_entityTypeName} {id}", _entityTypeName, id);

                return null;
            }
        }

        /// <inheritdoc/>
        public virtual async Task<Paging<TEntity>> GetPageData(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            int skip = (page - 1) * pageSize;
            var spec = new Specification<TEntity>(e => true, e => e.Id, false, skip, pageSize);

            try
            {
                var items = await Repository.GetBySpec(spec);
                var total = await Repository.CountBySpec(spec);

                return new Paging<TEntity>(items, total, page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get {pageSize} {_entityTypeName} on page {page}", _entityTypeName, pageSize, page);

                return new Paging<TEntity>();
            }
        }

        /// <inheritdoc/>
        public virtual async Task<Result<TEntity>> Save(TEntity entity, bool createIfNotExist = false, string userEmail = null, string userDisplayName = null)
        {
            bool createMode = false;

            TEntity item = null;
            if (entity.Id < 1)
            {
                if (!createIfNotExist)
                    return Result<TEntity>.FailedResult(entity, $"{_entityTypeName} was not created because the {nameof(createIfNotExist)} is \"false\"");

                createMode = true;
            }
            else
            {
                item = await Repository.GetById(entity.Id);
                if (item == null)
                {
                    if (!createIfNotExist)
                        return Result<TEntity>.FailedResult(entity, $"{_entityTypeName} (Id = {entity.Id}) doesn't exist");

                    createMode = true;
                }
            }

            if (createMode)
            {
                try
                {
                    _ = await Repository.Create(entity, userEmail, userDisplayName, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Failed to create {_entityTypeName}");

                    return Result<TEntity>.FailedResult(entity, $"Failed to create {_entityTypeName}. Please check the logs.");
                }

                Logger.LogInformation($"{_entityTypeName} has been created successfully");
            }
            else
            {
                try
                {
                    item.UpdateValueFrom(entity);
                    await Repository.Update(item, userEmail, userDisplayName);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Failed to update {_entityTypeName} ({Id})", _entityTypeName, entity.Id);

                    return Result<TEntity>.FailedResult(entity, $"Failed to update {_entityTypeName} (Id = {entity.Id}). Please check the logs.");
                }

                Logger.LogInformation("{_entityTypeName} {Id} has been updated successfully", _entityTypeName, entity.Id);
            }

            var updatedEntity = await Repository.GetById(entity.Id);

            return Result<TEntity>.SuccessResult(updatedEntity);
        }
    }
}
