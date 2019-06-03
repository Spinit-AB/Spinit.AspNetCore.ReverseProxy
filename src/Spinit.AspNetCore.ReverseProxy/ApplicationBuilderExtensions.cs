using System;
using Microsoft.AspNetCore.Builder;

namespace Spinit.AspNetCore.ReverseProxy
{
    public static class ApplicationBuilderExtensions
    {
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
