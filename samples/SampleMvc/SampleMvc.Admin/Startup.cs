using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polyrific.Project.Mvc;
using SampleMvc.Admin.Areas.Identity.Services;
using SampleMvc.Admin.Identity;
using SampleMvc.Core.Constants;
using SampleMvc.Infrastructure;

namespace SampleMvc.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddBaseProjectControllers(); // Add controllers from Polyrific base project

            services
                .RegisterDbContext(Configuration.GetConnectionString("DefaultConnection"))
                .RegisterRepositories()
                .RegisterServices();

            services.AddAppIdentity().AddDefaultUI();
            services.AddRazorPages();
            services.AddAuthentication();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicy.UserRoleAdminAccess, policy => policy.RequireRole(UserRole.Administrator));
                options.AddPolicy(AuthorizePolicy.UserRoleGuestAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Guest));
            });

            services.AddEmail(Configuration);
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
