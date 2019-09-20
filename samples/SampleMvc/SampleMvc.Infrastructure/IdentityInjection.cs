using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Data;
using Polyrific.Project.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleMvc.Infrastructure
{
    public static class IdentityInjection
    {
        public static IdentityBuilder AddAppIdentity(this IServiceCollection services)
        {
            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            return identityBuilder;
        }
    }
    
}
