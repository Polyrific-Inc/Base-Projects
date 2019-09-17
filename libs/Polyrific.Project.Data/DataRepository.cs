using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Project.Data
{
    public class DataRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext Db;

        public DataRepository(DbContext db)
        {
            Db = db;
        }

        public virtual async Task<int> CountBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return await secondaryResult.CountAsync(spec.Criteria, cancellationToken);
        }

        public virtual async Task<int> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Created = DateTime.UtcNow;
            Db.Set<TEntity>().Add(entity);
            await Db.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public virtual async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbSet = Db.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await Db.SaveChangesAsync(cancellationToken);
        }

        public virtual Task<TEntity> GetById(int id, CancellationToken cancellationToken = default)
        {
            return Db.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            }

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .Where(spec.Criteria)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Db.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // add order by to query
            if (spec.OrderBy != null)
            {
                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);
            }

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);
        }

        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            entity.Updated = DateTime.UtcNow;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync(cancellationToken);
        }
    }
}
