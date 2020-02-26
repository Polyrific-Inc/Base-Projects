using Core.Product;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Project.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityConfigs
{
    public class ProductConfig : BaseEntityConfig<ProductEntity>
    {
        public ProductConfig() : base("Products")
        {

        }

        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name).IsRequired();
        }
    }
}
