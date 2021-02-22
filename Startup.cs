using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Jobs;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Options;
using QuotesExchangeApp.Quartz;
using QuotesExchangeApp.Services.Grabbing;
using QuotesExchangeApp.Services.Interfaces.Grabbing;

namespace QuotesExchangeApp
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
            services.AddSignalR();
            services.ConfigureJwt(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlite(
                  Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();


            services.AddSingleton<IJobFactory, JobFactory>();

            services.Add(new ServiceDescriptor(typeof(FinnhubGrabberJob), typeof(FinnhubGrabberJob), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(MoexGrabberJob), typeof(MoexGrabberJob), ServiceLifetime.Transient));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();

                scheduler.Start();

                return scheduler;
            });
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.Configure<JwtOptions>(options => Configuration.GetSection("jwt").Bind(options));
            services.Configure<FinhubOptions>(options => Configuration.GetSection("finhub").Bind(options));

            services.AddTransient<IFinhubGrabberService, FinhubGrabberService>();
            services.AddTransient<IMoexGrabberService, MoexGrabberService>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<QuotesHub>("/hubs/quotes");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
