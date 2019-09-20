using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Data;
using SampleMvc.Data;

namespace SampleMvc.Infrastructure
{
    public static class DbContextInjection
    {
        /// <summary>
        /// Register DbContext to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">Connection string for the database</param>
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext, SampleDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }).AddDbContext<SampleDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
