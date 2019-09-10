using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreIdentityExample.Models.Entities;

namespace NetCoreIdentityExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {



            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCustomDbContext(Configuration);
            services.AddCustomIdentityOptions();
            services.AddCustomCookieOptions();
        }



        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
    static class CustomConfigureServiceExtentations
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityExampleContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityDbConnectionString")));

            return services;
        }

        public static IServiceCollection AddCustomIdentityOptions(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;

            })
            .AddEntityFrameworkStores<IdentityExampleContext>()
            .AddDefaultTokenProviders();

            return services;
        }


        public static IServiceCollection AddCustomCookieOptions(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Account/Login";
                cfg.AccessDeniedPath = "/Security/AccessDenied";
                cfg.SlidingExpiration = true;
                cfg.ExpireTimeSpan = TimeSpan.FromSeconds(5);
                cfg.Cookie = new CookieBuilder
                {
                    Domain = "DenemeCookie_Domain",
                    Expiration = TimeSpan.FromSeconds(10),
                    HttpOnly = true,
                    Name = "Deneme_Cookie_Name",
                    Path = "/"
                };

            });
            return services;
        }





    }
}
