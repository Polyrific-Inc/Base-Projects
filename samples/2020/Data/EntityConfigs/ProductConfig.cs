using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Project.Data;

namespace Data.EntityConfigs
{
    public class ProductConfig : BaseEntityConfig<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();
        }
    }
}