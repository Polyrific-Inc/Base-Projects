using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Data;
using SampleAngular.Core.Entities;
using SampleAngular.Core.Repositories;
using SampleAngular.Data;

namespace SampleAngular.Infrastructure
{
    public static class RepositoryInjection
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, DataRepository<Product>>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
