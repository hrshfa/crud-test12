using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CurrieTechnologies.Razor.SweetAlert2;
using Mc2.CrudTest.Presentation.Front.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Mc2.CrudTest.Presentation.Front
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();
           
            builder.Services.AddTransient<AntiforgeryHandler>();
            builder.Services.AddHttpClient("backend", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<AntiforgeryHandler>();
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("backend"));

            builder.Services.AddScoped<DialogAndNotificationService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<ICustomersService, CustomersService>();

            builder.Services.AddSweetAlert2(options =>
            {
                options.Theme = SweetAlertTheme.Minimal;
                options.SetThemeForColorSchemePreference(ColorScheme.Light, SweetAlertTheme.Default);
                options.SetThemeForColorSchemePreference(ColorScheme.Dark, SweetAlertTheme.Dark);
            });
            await builder.Build().RunAsync();
        }
    }
}
