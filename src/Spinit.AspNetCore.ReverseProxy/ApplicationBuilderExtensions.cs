using System;
using Microsoft.AspNetCore.Builder;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    ///  Extension methods for adding <see cref="IReverseProxy"/> middleware to an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="RouterMiddleware"/> with routes used by the <see cref="IReverseProxy"/> to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="configure">An <see cref="Action{T}"/> to configure the provided <see cref="IReverseProxyRouteBuilder"/>.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseReverseProxy(this IApplicationBuilder applicationBuilder, Action<IReverseProxyRouteBuilder> configure)
        {
            var routes = new ReverseProxyRouteBuilder(applicationBuilder);
            configure(routes);
            return applicationBuilder
                .UseRouter(routes.Build())
                ;
        }
    }
}
