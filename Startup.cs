
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediaApplication.Models;
using Microsoft.AspNetCore.Http;
using MediaApplication.Data;
using MediaApplication.Services;
using AutoMapper;
using NLog.Extensions.Logging;
using NLog.Config;
using System;

namespace MediaApplication
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();


            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var connection = @"User ID=sa;Password=Artesian1@34;Initial Catalog=movieDb;Data Source=192.168.0.234;Integrated Security=false;";
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));


            services.Configure<IdentityOptions>(options =>
               {
                   // Password settings
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 8;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireLowercase = false;

                   // Lockout settings
                   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                   options.Lockout.MaxFailedAccessAttempts = 2;

                   // Cookie settings
                   options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                   options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                   options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                   // User settings
                   options.User.RequireUniqueEmail = true;
               });
            services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();
            services.AddMvc();
            

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IMovieService, MovieService>();
            services.AddSingleton<IMapper>(sp => _mapperConfiguration.CreateMapper());

            services.AddAntiforgery();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            AspNetCoreBaseDirLayoutRenderer.Env = env;
            ConfigurationItemFactory.Default.LayoutRenderers
                     .RegisterDefinition("basedir", typeof(AspNetCoreBaseDirLayoutRenderer));
            env.ConfigureNLog("nlog.config");

            // app.Run(async context1 =>
            // {
            //          await context1.Response.WriteAsync(env.ContentRootPath.ToString());
            // });
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseDatabaseErrorPage();
            //     app.UseBrowserLink();
            // }
            // else
            // {
            //     app.UseExceptionHandler("/Home/Error");
            // }

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseBrowserLink();

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Movies}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context);
        }
    }
}
