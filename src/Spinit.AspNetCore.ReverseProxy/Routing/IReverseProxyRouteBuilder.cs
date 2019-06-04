using System;
using Microsoft.AspNetCore.Routing;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Defines a contract for a reverse proxy route builder in an application. 
    /// A reverse proxy route builder specifies the routes used to call the <see cref="ReverseProxy"/>.
    /// </summary>
    public interface IReverseProxyRouteBuilder
    {
        /// <summary>
        /// Adds a route to the <see cref="IReverseProxyRouteBuilder"/> template and uri rewrite function.
        /// </summary>
        /// <param name="routeTemplate">The URL pattern of the route.</param>
        /// <param name="rewrite">
        ///     A function that should return the proxy uri given the <see cref="RouteData"/> from the <paramref name="routeTemplate"/>. 
        ///     <para>Returning an absolute <see cref="Uri"/> is recommended.</para>
        /// </param>
        /// <returns></returns>
        IReverseProxyRouteBuilder MapRoute(string routeTemplate, Func<RouteData, Uri> rewrite);
    }
}
