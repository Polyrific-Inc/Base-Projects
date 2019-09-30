using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Constants;
using Polyrific.Project.Data;
using Polyrific.Project.Data.Identity;
using System;

namespace Polyrific.Project.Mvc
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBaseProjectServices(this IServiceCollection services, 
            IConfiguration configuration,
            Action<ServicesOptions> configure)
        {
            var options = new ServicesOptions();
            configure(options);

            // add mvc
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Add identity
            if (options.EnableIdentity)
            {
                var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();

                if (options.UseDefaultUIForIdentity)
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

                services.AddAuthentication();

                // Add user service
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddTransient<IUserService, UserService>();
            }

            // Add Email service
            if (options.EnableSmtpEmailSender)
            {
                services.AddTransient<IEmailSender, EmailSender>();

                var section = configuration.GetSection("SmtpSetting");
                services.Configure<SmtpSetting>(section);
            }

            services.AddAutoMapper(typeof(ServiceCollectionExtension).Assembly, typeof(IdentityAutoMapperProfile).Assembly);

            return services;
        }
    }
}
