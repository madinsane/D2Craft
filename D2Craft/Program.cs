using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D2Craft
{
    public class Program
    {
        private static async Task DebugDelayAsync()
        {
#if DEBUG
            await Task.Delay(5000);
#endif
        }

        public static async Task Main(string[] args)
        {
            await DebugDelayAsync();
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<StateContainer>();
            await builder.Build().RunAsync();
        }
    }
}
