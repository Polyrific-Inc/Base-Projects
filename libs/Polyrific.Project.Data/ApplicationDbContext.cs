using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Data.Identity;

namespace Polyrific.Project.Data
{
    public abstract class ApplicationDbContext<TUser> : IdentityDbContext<TUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
        where TUser: ApplicationUser
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected ApplicationDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfig());
        }
    }

    public abstract class ApplicationDbContext: ApplicationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected ApplicationDbContext()
        {

        }
    }
}