using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SampleMvc.Data;
using SampleMvc.Data.Identity;

namespace SampleMvc.Infrastructure
{
    public static class IdentityInjection
    {
        public static IdentityBuilder AddAppIdentity(this IServiceCollection services)
        {
            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<SampleDbContext>();
            
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            return identityBuilder;
        }
    }
}
