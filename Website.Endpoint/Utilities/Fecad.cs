using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.UriComposer;
using Application.Catalogs.GetMenuItem;
using Application.Interfaces.Context;
using Application.Visitors.SaveVisitorInfo;
using Application.Visitors.VisitorOnline;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Context.MongoContext;
using System;
using Website.Endpoint.Utilities.Filters;
using Infrastructure.MappingProfile;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Application.BasketService;

namespace Website.Endpoint.Utilities
{
    public static class Facade
    {

        public static IServiceCollection AddServiceProject(this IServiceCollection services,
            IConfiguration configuration)
        {

            #region ConnectionString
            string connection = configuration["ConnectionString:SqlServerCnn"];
            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(connection));
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            #endregion
             
            services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            }); 
            services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
            services.AddTransient<IVisitorOnlineService, VisitorOnlineService>();
            services.AddTransient<IGetMenuItemService, GetMenuItemService>();
            services.AddTransient<IGetCatalogIItemPLPService, GetCatalogIItemPLPService>();
            services.AddTransient<IGetCatalogItemPDPService, GetCatalogItemPDPService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IUriComposerService, UriComposerService>(); 
            services.AddScoped<SaveVisitorFilter>();
            services.AddAutoMapper(typeof(CatalogMappingProfile));
            services.AddSignalR();
            services.AddAuthentication();

            return services;
        }
    }
}
