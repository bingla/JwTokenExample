using System;
using JwToken.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JwToken.Extensions;
using JwToken.Interfaces;
using JwToken.Services;

namespace JwToken
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF Context and Controllers
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("apiDb"));
            services.AddControllers();

            // Add IHttpContextAccessor so you can access HttpContext from a service with IoC
            services.AddHttpContextAccessor();

            // Add Cors if needed
            services.AddCors();

            // Add JwToken-stuff
            var secret = Configuration.GetValue<string>("Auth:JwTSecret");
            services.AddJwToken(secret);

            // Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var context = serviceProvider.GetService<ApiContext>();
            SeedTestData.SeedUsers(context);

            app.UseHttpsRedirection();
            app.UseRouting();

            // Use Cors with jwt if needed
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            // UseAuthentication must appear BEFORE UseAuthorization for JWT to work!! >:(
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
