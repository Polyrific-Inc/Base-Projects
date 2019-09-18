using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SampleMvc.Data;
using SampleMvc.Data.Identity;

namespace SampleMvc.Infrastructure
{
    public static class IdentityInjection
    {
        public static void AddAppIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<SampleDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
        }
    }
}
