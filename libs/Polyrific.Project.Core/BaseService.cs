using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Polyrific.Project.Core
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;

        private readonly string _entityName;

        protected BaseService(IRepository<T> repository, ILogger logger)
        {
            Logger = logger;
            _repository = repository;
            _entityName = typeof(T).Name;
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
                Logger.LogError(ex, "Failed to delete {_entityName} {id}", _entityName, id);

                return Result.FailedResult($"Failed to delete {_entityName} {id}. Please check the logs.");
            }

            Logger.LogInformation("{_entityName} {webClientId} has been deleted successfully", _entityName, id);

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
                Logger.LogError(ex, "Failed to get {_entityName} {id}", _entityName, id);

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
                Logger.LogError(ex, "Failed to get {pageSize} {_entityName} on page {page}", _entityName, pageSize, page);

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
                    return Result<T>.FailedResult(entity, $"{_entityName} was not created because the {nameof(createIfNotExist)} is \"false\"");

                createMode = true;
            }
            else
            {
                item = await _repository.GetById(entity.Id);
                if (item == null)
                {
                    if (!createIfNotExist)
                        return Result<T>.FailedResult(entity, $"{_entityName} {entity.Id} doesn't exist");

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
                    Logger.LogError(ex, $"Failed to create {_entityName}");

                    return Result<T>.FailedResult(entity, $"Failed to create {_entityName}. Please check the logs.");
                }

                Logger.LogInformation($"{_entityName} has been created successfully");
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
                    Logger.LogError(ex, "Failed to update entity {Id}", entity.Id);

                    return Result<T>.FailedResult(entity, $"Failed to update {_entityName} {entity.Id}. Please check the logs.");
                }

                Logger.LogInformation("{_entityName} {Id} has been updated successfully", _entityName, entity.Id);
            }

            var updatedEntity = await Get(entity.Id);

            return Result<T>.SuccessResult(updatedEntity);
        }
    }
}
