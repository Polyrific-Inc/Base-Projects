using Microsoft.Extensions.DependencyInjection;
using SampleAngular.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAngular.Infrastructure
{
    public static class ServiceInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();

            return services;
        }
    }
}
