using Microsoft.EntityFrameworkCore;
using SampleMvc.Core.Entities;
using SampleMvc.Data.EntityConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleMvc.Data
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
