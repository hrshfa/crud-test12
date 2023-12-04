using Mc2.CrudTest.Presentation.Data.Context;
using Mc2.CrudTest.Presentation.Data.Repositories;
using Mc2.CrudTest.Presentation.Server.Services;
using Mc2.CrudTest.Shared.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Text.Json;

namespace Mc2.CrudTest.Presentation.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllersWithViews();

            services.AddRazorPages();
            var connectionString = Configuration.GetConnectionString("WebApiDatabase");
            services.AddDbContext<PostgreSQLDbContext>(options =>
            {
                options.UseNpgsql(connectionString, opt =>
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    opt.CommandTimeout(minutes);
                    opt.EnableRetryOnFailure();
                });
            });

            services.AddScoped<IDbInitializerService, DbInitializerService>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddSingleton<RabbitMQProducerService>();

            services.AddBff();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "__Host-blazor";
                    options.Cookie.SameSite = SameSiteMode.Strict;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://demo.duendesoftware.com";

                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";
                    options.AccessDeniedPath = "/";
                    options.SignedOutCallbackPath = "/";
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("api");
                    options.Scope.Add("offline_access");

                    options.MapInboundClaims = false;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                    using var scope = scopeFactory.CreateScope();

                    var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    var message = ErrorHandlingHelper.ParseError(_logger, "UseExceptionHandler", error?.Error);

                    if (error?.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            State = 500,
                            Msg = error.Error.Message
                        }));
                    }
                    else
                    {
                        await next();
                    }
                });
            });

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseBff();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers()
                .RequireAuthorization()
                .AsBffApiEndpoint();

                endpoints.MapBffManagementEndpoints();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
