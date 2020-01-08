using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleAngular.Api.Identity
{
    public static class EmailInjection
    {
        public static void AddEmail(this IServiceCollection services, IConfiguration configuration, string sectionName = "SmtpSetting")
        {
            services.AddTransient<IEmailSender, EmailSender>();

            var section = configuration.GetSection(sectionName);
            services.Configure<SmtpSetting>(section);
        }
    }
}
