using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SampleAngular.Api.Identity;
using SampleAngular.Core.Constants;
using SampleAngular.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SampleAngular.Api
{
    public class Startup
    {
        private readonly string _allowSpecificOriginsPolicy = "_allowSpecificOriginsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .RegisterDbContext(Configuration.GetConnectionString("DefaultConnection"))
                .RegisterRepositories()
                .RegisterServices();

            services.AddAutoMapper(typeof(Startup).Assembly, typeof(Data.SampleDbContext).Assembly);

            services.AddEmail(Configuration);
            services.AddAppIdentity();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Security:Tokens:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Security:Tokens:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Tokens:Key"])),
                        RequireExpirationTime = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicy.UserRoleAdminAccess, policy => policy.RequireRole(UserRole.Administrator));
                options.AddPolicy(AuthorizePolicy.UserRoleGuestAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Guest));
            });

            services.AddCors(options =>
            {
                options.AddPolicy(_allowSpecificOriginsPolicy, builder => builder
                    .WithOrigins(Configuration["AllowedOrigin"].Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(_allowSpecificOriginsPolicy);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
