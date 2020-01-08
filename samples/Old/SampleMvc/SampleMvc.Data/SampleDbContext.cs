using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleMvc.Core.Entities;
using SampleMvc.Data.EntityConfigs;
using SampleMvc.Data.Identity;

namespace SampleMvc.Data
{
    public class SampleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
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

            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfig());
        }
    }
}
