﻿using AutoMapper;
using Hisar.Component.Guideline.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreStack.Data.Context;
using NetCoreStack.Hisar;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;
using Shared.Library;
using System.IO;
using System.Threading.Tasks;

namespace Hisar.Component.Guideline
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
#if !RELEASE
            services.AddCliSocket<Startup>();
#endif
            services.AddSingleton<SampleService>();

            services.AddComposers(options =>
            {
                options.CreateMap<AlbumViewModel, AlbumViewModelComposer>();
                options.CreateMap<GenreViewModel, AnotherComposer>();
            });

            services.AddHisarMongoDbContext<MongoDbContext>(Configuration);
            services.AddAutoMapper();

            services.AddMenuRenderer<SharedMenuItemsRenderer>();

            services.AddTransient<IHisarExceptionFilter, GuidelineExceptionFilter>();

            services.AddCacheValueProviders(setup =>
            {
                setup.DefaultMap<AlbumBson, CustomCacheValueProvider>();
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

#if !RELEASE
            app.UseCliProxy();
#endif

            Task.Run(() => BsonDataInitializer.InitializeMusicStoreMongoDb(app.ApplicationServices));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(ConfigureRoutes);
        }

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args).Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<DefaultHisarStartup<Startup>>()
                .Build();

            host.Run();
        }


        public static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "req",
                template: "registration",
                defaults: new { controller = "Home", action = "Registration" });

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

    }
}