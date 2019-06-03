using System;
using Microsoft.Extensions.DependencyInjection;

namespace Spinit.AspNetCore.ReverseProxy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReverseProxy(this IServiceCollection serviceCollection, Action<ReverseProxyOptions> configure)
        {
            serviceCollection
                .AddOptions<ReverseProxyOptions>()
                .Configure(configure);
            
            serviceCollection
                .AddSingleton<IReverseProxy, ReverseProxy>()
                .AddHttpClient<IReverseProxy, ReverseProxy>();
            return serviceCollection;
        }
    }
}
