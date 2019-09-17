using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Project.Core;
using System;

namespace Polyrific.Project.Data
{
    public abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(e => e.Created).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(e => e.Updated).HasConversion(v => v, v => v != null ? (DateTime?)DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);
        }
    }
}
