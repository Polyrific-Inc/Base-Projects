using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Constants;
using Polyrific.Project.Data;
using Polyrific.Project.Data.Identity;

namespace Polyrific.Project.Mvc
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBaseProject(this IServiceCollection services, 
            IConfiguration configuration,
            bool useDefaultUIForIdentity = true)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.ConfigureOptions(typeof(BaseProjectConfigureOptions));

            // add mvc
            var assembly = typeof(ServiceCollection).Assembly;
            services.AddMvc().AddApplicationPart(assembly);

            // Add identity
            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            if (useDefaultUIForIdentity)
            {
                identityBuilder.AddDefaultUI();
            }

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            // Add authorization policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicy.UserRoleAdminAccess, policy => policy.RequireRole(UserRole.Administrator));
                options.AddPolicy(AuthorizePolicy.UserRoleGuestAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Guest));
            });

            // Add user service
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            // Add Email service
            services.AddTransient<IEmailSender, EmailSender>();

            var section = configuration.GetSection("SmtpSetting");
            services.Configure<SmtpSetting>(section);

            return services;
        }
    }
}
