using Microsoft.Extensions.DependencyInjection;

namespace Polyrific.Project.Mvc
{
    /// <summary>
    /// The extension class for the mvc project
    /// </summary>
    public static class MvcBuilderExtension
    {
        /// <summary>
        /// Add controllers from this base project
        /// </summary>
        /// <param name="builder">An <see cref="IMvcBuilder"/> that can be used to further configure the MVC services.</param>
        /// <returns></returns>
        public static IMvcBuilder AddBaseProjectControllers(this IMvcBuilder builder)
        {
            builder.AddApplicationPart(typeof(MvcBuilderExtension).Assembly);

            return builder;
        }
    }
}