using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Model.GraphQL;
using StepChallenge.Controllers;
using StepChallenge.Mutation;
using StepChallenge.Query;
using StepChallenge.Services;

namespace StepChallenge
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<StepContext>();
            services.AddTransient<StepsService>();
            services.AddTransient<UserService>();
            services.AddTransient<TeamService>();
            services.AddSingleton<ParticipantType>();
            services.AddSingleton<UserType>();
            services.AddSingleton<TeamType>();
            services.AddSingleton<StepsType>();
            services.AddSingleton<StepInputType>();
            services.AddSingleton<LeaderBoardType>();
            services.AddSingleton<TeamScoreType>();
            services.AddSingleton<StepChallengeMutation>();
            services.AddSingleton<StepChallengeQuery>();
            services.AddSingleton<StepChallengeQuery>();
            services.AddSingleton<ChallengeSettingsType>();
            services.AddSingleton<ChallengeSettingsInputType>();
            services.AddSingleton<TeamInputType>();
            services.AddSingleton<AdminLeaderBoardType>();
            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new StepSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".StepChallenge.Session";
                // Set a short timeout for easy testing
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<StepContext>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddSessionStateTempDataProvider();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                        options.User.RequireUniqueEmail = false;
                    });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = string.Empty;
                options.AccessDeniedPath = string.Empty;
                options.LogoutPath = string.Empty;
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;    
                    return Task.CompletedTask;
                };
            });
            
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
            
            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();
                authSettings.AddPolicy("AdminPolicy", _ => _.RequireClaim("Role", "Admin"));
                return authSettings;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseGraphiQl("/graphql");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<StepContext>();
                db.Database.Migrate();
                var ideManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var dataSeed = new DataSeed(ideManager, roleManager);
                await dataSeed.Run(db);

                await dataSeed.SetupRoles();

            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }

}
