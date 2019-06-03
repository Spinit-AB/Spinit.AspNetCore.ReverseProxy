using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class ReverseProxyRouteHandler : IRouter, IRouteHandler
    {
        public ReverseProxyRouteHandler(Func<RouteData, Uri> rewriter) 
        {
            Rewriter = rewriter;
        }

        public Func<RouteData, Uri> Rewriter { get; }

        public RequestDelegate GetRequestHandler(HttpContext httpContext, RouteData routeData)
        {
            return async handler =>
            {
                var proxyUri = Rewriter(routeData);
                var reverseProxy = httpContext.RequestServices.GetRequiredService<IReverseProxy>();
                var response = await reverseProxy.ExecuteAsync(httpContext.Request, proxyUri).ConfigureAwait(false);
                await httpContext.Response.AssignAsync(response);
            };
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            context.Handler = GetRequestHandler(context.HttpContext, context.RouteData);
            return Task.CompletedTask;
        }
    }
}
