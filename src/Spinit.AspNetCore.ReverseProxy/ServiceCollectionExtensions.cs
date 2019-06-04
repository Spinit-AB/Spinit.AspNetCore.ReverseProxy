using System;
using Microsoft.Extensions.DependencyInjection;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Extensions methods to configure an <see cref="IServiceCollection"/> for <see cref="IReverseProxy"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="IReverseProxy"/> and related services to the <see cref="IServiceCollection"/> using the supplied options.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configure">Optional <see cref="Action{T}"/> to configure the provided <see cref="ReverseProxyOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddReverseProxy(this IServiceCollection serviceCollection, Action<ReverseProxyOptions> configure = null)
        {
            serviceCollection
                .AddOptions<ReverseProxyOptions>();

            if (configure != null)
                serviceCollection.Configure(configure);
            
            serviceCollection
                .AddSingleton<IReverseProxy, DefaultReverseProxy>()
                .AddHttpClient<IReverseProxy, DefaultReverseProxy>();
            return serviceCollection;
        }
    }
}
