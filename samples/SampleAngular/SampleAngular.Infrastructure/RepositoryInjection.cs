using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Data;
using SampleAngular.Core.Entities;

namespace SampleAngular.Infrastructure
{
    public static class RepositoryInjection
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, DataRepository<Product>>();

            return services;
        }
    }
}
