
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediaApplication.Models;
using MediaApplication.Data;
using MediaApplication.Services;
using AutoMapper;
using NLog.Extensions.Logging;
using NLog.Config;
using System;
using Microsoft.IdentityModel.Tokens;
using MediaApplication.JWTMiddleware;
using Microsoft.Extensions.Options;
using System.Text;

namespace MediaApplication
{
    public class Startup
    {

        private static readonly string secretKey = "supersecret_secretkey!123";
        private static readonly string issure = "Movie-Application";
        private static readonly string audience = "All-Audience";
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

            // services.AddIdentityServer()
            //    .AddTemporarySigningCredential()
            //    .AddInMemoryPersistedGrants()
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryApiResources(Config.GetApiResources())
            //    .AddInMemoryClients(Config.GetClients())
            //    .AddAspNetIdentity<ApplicationUser>();

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




            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            //generate token

            var jwtOptions = new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issure,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(jwtOptions));

            //validation key


            var tokenValidationParameters = new TokenValidationParameters
            {
                //The signing key must match !
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                //Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issure,

                //validate the JWT Audience (aud) claim

                ValidateAudience = true,
                ValidAudience = audience,

                //validate the token expiry
                ValidateLifetime = true,

                // If you  want to allow a certain amout of clock drift
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                AuthenticationScheme = "Bearer"
            });

         
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
            //app.UseIdentityServer();

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
