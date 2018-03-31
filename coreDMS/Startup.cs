using CoreDMS.Model;
using CoreDMS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CoreDMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IHostingEnvironment env)
        {
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var uploads = Configuration.GetValue<string>("uploads");
            var processed = Configuration.GetValue<string>("processed");
            var connection = Configuration.GetValue<string>("dbFile");

            services.AddMvc();
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

            services.AddDbContext<DMSContext>(options =>
                options.UseSqlite(connection)
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
