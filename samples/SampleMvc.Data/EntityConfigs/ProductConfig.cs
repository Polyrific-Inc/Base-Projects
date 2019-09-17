using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Project.Data;
using SampleMvc.Core.Entities;

namespace SampleMvc.Data.EntityConfigs
{
    public class ProductConfig : BaseEntityConfig<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name).IsRequired();
        }
    }
}
