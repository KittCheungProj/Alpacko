using Alpacko.Client.Web.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alpacko.Client.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44396/") });
            builder.Services.AddTransient(sp => new HttpClient(new DefaultBrowserOptionsMessageHandler(new WebAssemblyHttpHandler())
            {
                DefaultBrowserRequestCache = BrowserRequestCache.NoCache,
                DefaultBrowserRequestCredentials = BrowserRequestCredentials.Omit,
                DefaultBrowserRequestMode = BrowserRequestMode.Cors,
            })
            {
                BaseAddress = new Uri("https://localhost:44396/"),
            });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            await builder.Build().RunAsync();
        }
    }
}
