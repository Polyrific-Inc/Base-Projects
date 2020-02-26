using Core.Product;
using Microsoft.Extensions.DependencyInjection;
using Polyrific.Project.Core;
using Polyrific.Project.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DataRepositorySetup
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ProductEntity>, DataRepository<ProductEntity>>();

            return services;
        }
    }
}
