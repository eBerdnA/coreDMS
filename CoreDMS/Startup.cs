using CoreDMS.Model;
using CoreDMS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Serilog;
using Serilog.Core;
using System;
using System.IO;

namespace CoreDMS
{
    public class Startup
    {
        Logger logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var logDir = Configuration.GetValue<string>(ConfigKeys.LogDir);

            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(logDir, "consoleapp.log"))
                .CreateLogger();
        }

        public Startup(IHostEnvironment env)
        {
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var uploads = Configuration.GetValue<string>(ConfigKeys.UploadsDir);
            var processed = Configuration.GetValue<string>(ConfigKeys.ProcessedDir);
            var connection = Configuration.GetValue<string>(ConfigKeys.DbFilePath);
            System.Console.WriteLine(connection);
            logger.Information($"uploads: {uploads}");
            logger.Information($"processed: {processed}");

            services.AddMvc(option => option.EnableEndpointRouting = false)
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISettings>(new Settings(uploads, processed));
            if (string.IsNullOrEmpty(connection))
            {
                connection = @"Datasource=db.sqlite";
            }
            else
            {
                connection = $"Datasource={connection}";
            }
            logger.Information($"connection: {connection}");
            services.AddDbContext<DMSContext>(options =>
                options.UseSqlite(connection)
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMiddleware<ReflectionTypeLoadExceptionLoggingMiddleware>();
            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");                
            // });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    // spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
