using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;

        private readonly string _entityTypeName;

        protected BaseService(IRepository<T> repository, ILogger logger)
        {
            Logger = logger;
            _repository = repository;
            _entityTypeName = typeof(T).Name;
        }

        protected ILogger Logger { get; }

        public virtual async Task<Result> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete {_entityTypeName} {id}", _entityTypeName, id);

                return Result.FailedResult($"Failed to delete {_entityTypeName} {id}. Please check the logs.");
            }

            Logger.LogInformation("{_entityTypeName} {webClientId} has been deleted successfully", _entityTypeName, id);

            return Result.SuccessResult;
        }

        public virtual async Task<T> Get(int id)
        {
            try
            {
                return await _repository.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get {_entityTypeName} {id}", _entityTypeName, id);

                return null;
            }
        }

        public virtual async Task<Paging<T>> GetPageData(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            int skip = (page - 1) * pageSize;
            var spec = new Specification<T>(e => true, e => e.Id, false, skip, pageSize);

            try
            {
                var items = await _repository.GetBySpec(spec);
                var total = await _repository.CountBySpec(spec);

                return new Paging<T>(items, total, page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get {pageSize} {_entityTypeName} on page {page}", _entityTypeName, pageSize, page);

                return new Paging<T>();
            }
        }

        public async Task<Result<T>> Save(T entity, bool createIfNotExist = false)
        {
            bool createMode = false;

            T item = null;
            if (entity.Id < 1)
            {
                if (!createIfNotExist)
                    return Result<T>.FailedResult(entity, $"{_entityTypeName} was not created because the {nameof(createIfNotExist)} is \"false\"");

                createMode = true;
            }
            else
            {
                item = await _repository.GetById(entity.Id);
                if (item == null)
                {
                    if (!createIfNotExist)
                        return Result<T>.FailedResult(entity, $"{_entityTypeName} {entity.Id} doesn't exist");

                    createMode = true;
                }
            }

            if (createMode)
            {
                try
                {
                    _ = await _repository.Create(entity);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Failed to create {_entityTypeName}");

                    return Result<T>.FailedResult(entity, $"Failed to create {_entityTypeName}. Please check the logs.");
                }

                Logger.LogInformation($"{_entityTypeName} has been created successfully");
            }
            else
            {
                try
                {
                    item.UpdateValueFrom(entity);
                    await _repository.Update(item);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Failed to update {_entityTypeName} {Id}", _entityTypeName,entity.Id);

                    return Result<T>.FailedResult(entity, $"Failed to update {_entityTypeName} {entity.Id}. Please check the logs.");
                }

                Logger.LogInformation("{_entityTypeName} {Id} has been updated successfully", _entityTypeName, entity.Id);
            }

            var updatedEntity = await Get(entity.Id);

            return Result<T>.SuccessResult(updatedEntity);
        }
    }
}
