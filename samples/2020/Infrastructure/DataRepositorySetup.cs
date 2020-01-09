using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Data;

namespace Infrastructure
{
    public static class DataRepositorySetup
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, DataRepository<Product>>();

            return services;
        }
    }
}