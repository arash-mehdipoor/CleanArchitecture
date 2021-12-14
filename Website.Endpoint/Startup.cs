using Application.Interfaces.Context;
using Application.Visitors.SaveVisitorInfo;
using Infrastructure.IdentityConfigs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Context;
using Persistence.Context.MongoContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Endpoint.Utilities.Filters;

namespace Website.Endpoint
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
            services.AddControllersWithViews();

            #region ConnectionString
            string connection = Configuration["ConnectionString:SqlServerCnn"];
            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(connection));
            #endregion
            services.AddIdentityService(Configuration);
            services.AddAuthentication();
            services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            });

            services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
            services.AddScoped<SaveVisitorFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
