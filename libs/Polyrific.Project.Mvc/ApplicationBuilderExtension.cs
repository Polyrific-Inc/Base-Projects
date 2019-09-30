using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Mvc
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseBaseProjectServices(this IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute("area", "Account", "Account/{controller=User}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();
            });

            return app;
        }
    }
}
