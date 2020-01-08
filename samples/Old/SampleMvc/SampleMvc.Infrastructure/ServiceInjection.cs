using Microsoft.Extensions.DependencyInjection;
using SampleMvc.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleMvc.Infrastructure
{
    public static class ServiceInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
