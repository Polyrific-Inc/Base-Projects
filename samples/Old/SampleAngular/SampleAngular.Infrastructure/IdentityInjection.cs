using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SampleAngular.Data;
using SampleAngular.Data.Identity;

namespace SampleAngular.Infrastructure
{
    public static class IdentityInjection
    {
        /// <summary>
        /// Add Identity system to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="dbProvider">Database provider (either `mssql` or `sqlite`)</param>
        public static void AddAppIdentity(this IServiceCollection services)
        {

            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SampleDbContext>();
        }
    }
}
