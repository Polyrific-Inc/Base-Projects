using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Mvc
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBaseProject(this IServiceCollection services)
        {

            return services;
        }
    }
}
