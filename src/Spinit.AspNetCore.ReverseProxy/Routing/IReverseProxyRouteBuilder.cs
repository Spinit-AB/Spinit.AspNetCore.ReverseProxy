using System;
using Microsoft.AspNetCore.Routing;

namespace Spinit.AspNetCore.ReverseProxy
{
    public interface IReverseProxyRouteBuilder
    {
        IReverseProxyRouteBuilder MapRoute(string template, Func<RouteData, Uri> rewrite);
    }
}
