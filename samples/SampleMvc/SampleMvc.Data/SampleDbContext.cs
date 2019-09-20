using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Data;
using SampleMvc.Core.Entities;
using SampleMvc.Data.EntityConfigs;

namespace SampleMvc.Data
{
    public class SampleDbContext : ApplicationDbContext
    {
        public SampleDbContext(DbContextOptions options)
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
