using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class ReverseProxyRouteBuilder : RouteBuilder, IReverseProxyRouteBuilder
    {
        public ReverseProxyRouteBuilder(IApplicationBuilder applicationBuilder) 
            : base(applicationBuilder)
        {
        }

        public ReverseProxyRouteBuilder(IApplicationBuilder applicationBuilder, IRouter defaultHandler) 
            : base(applicationBuilder, defaultHandler)
        {
        }

        public IReverseProxyRouteBuilder MapRoute(string template, Func<RouteData, Uri> handler)
        {
            var route = new Route(
                new ReverseProxyRouteHandler(handler),
                template,
                defaults: null,
                constraints: null,
                dataTokens: null,
                inlineConstraintResolver: GetConstraintResolver(this));

            this.Routes.Add(route);
            return this;
        }

        private static IInlineConstraintResolver GetConstraintResolver(IRouteBuilder builder)
        {
            return builder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();
        }
    }
}
