using Microsoft.EntityFrameworkCore;
using SampleAngular.Core.Entities;
using SampleAngular.Data.EntityConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAngular.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfig());
        }
    }
}
