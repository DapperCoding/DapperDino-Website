using System;
using System.Threading.Tasks;
using AutoMapper;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDino
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<DiscordBuilder>();

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper();
            services.AddCors(options =>
                {
                    // BEGIN01
                    options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://example.com", "http://www.contoso.com");
                    });
                    // END01

                    // BEGIN02
                    options.AddPolicy("AllowAllOrigins",
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                    // END02

                    // BEGIN03
                    options.AddPolicy("AllowSpecificMethods",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .WithMethods("GET", "POST", "HEAD");
                        });
                    // END03

                    // BEGIN04
                    options.AddPolicy("AllowAllMethods",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .AllowAnyMethod();
                        });
                    // END04

                    // BEGIN05
                    options.AddPolicy("AllowHeaders",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .WithHeaders("accept", "content-type", "origin", "x-custom-header");
                        });
                    // END05

                    // BEGIN06
                    options.AddPolicy("AllowAllHeaders",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .AllowAnyHeader();
                        });
                    // END06

                    // BEGIN07
                    options.AddPolicy("ExposeResponseHeaders",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .WithExposedHeaders("x-custom-header");
                        });
                    // END07

                    // BEGIN08
                    options.AddPolicy("AllowCredentials",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .AllowCredentials();
                        });
                    // END08

                    // BEGIN09
                    options.AddPolicy("SetPreflightExpiration",
                        builder =>
                        {
                            builder.WithOrigins("http://example.com")
                                   .SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                        });
                    // END09
                });

            services.AddAntiforgery();
            services.AddSession();
            //Password Strength Setting
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Add a singleton for IActionContextAccessor
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Register SendGrid EmailSender used by account confirmation and password reset during
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseCors("AllowAllOrigins");

            app.UseSignalR(routes =>
            {
                routes.MapHub<DiscordBotHub>("/discordBotHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            ApplicationUser user = await UserManager.FindByEmailAsync("syedshanumcain@gmail.com");
            var User = new ApplicationUser();
            await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
