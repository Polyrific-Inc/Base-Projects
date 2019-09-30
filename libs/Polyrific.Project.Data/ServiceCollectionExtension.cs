using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Data
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services, string connectionString)
            where TDbContext : ApplicationDbContext
        {
            services.AddDbContext<ApplicationDbContext, TDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }).AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
