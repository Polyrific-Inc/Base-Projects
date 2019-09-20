using Microsoft.Extensions.DependencyInjection;
using SampleMvc.Core.Services;

namespace SampleMvc.Infrastructure
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
