using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleMvc.Core.Constants;

namespace SampleMvc.Data.Identity
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasData(CreateInitialUser());
        }

        private ApplicationUser CreateInitialUser()
        {
            var user = new ApplicationUser(1, "brain.konasara@polyrific.com")
            {
                EmailConfirmed = true,
                IsActive = true,
                PasswordHash = "AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==",

                // ideally these values don't need to be set here,
                // it's just a workaround because of a bug in ef core 2.1 which prevents migrations to work as expected
                ConcurrencyStamp = "6e60fade-1c1f-4f6a-ab7e-768358780783",
                SecurityStamp = "D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK"
            };

            return user;
        }
    }

    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("Roles");

            builder.HasData(
                new ApplicationRole(1, UserRole.Administrator){ConcurrencyStamp = "f8025fee-dec6-4528-9514-58339adc3383"},
                new ApplicationRole(2, UserRole.Guest) {ConcurrencyStamp = "18f44ef4-86b2-4ebb-a400-b2615c9715e0" }
            );
        }
    }

    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasData(new ApplicationUserRole(1, 1));
        }
    }

    public class ApplicationUserClaimConfig : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable("UserClaims");
        }
    }

    public class ApplicationUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable("UserLogins");
        }
    }

    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("RoleClaims");
        }
    }

    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.ToTable("UserTokens");
        }
    }
}
