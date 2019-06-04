using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Provides the context for the <see cref="IReverseProxyFilter.OnExecutingAsync(ReverseProxyExecutingContext)"/> method.
    /// </summary>
    public class ReverseProxyExecutingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseProxyExecutingContext"/> class.
        /// </summary>
        /// <param name="incomingRequest"></param>
        /// <param name="proxyRequest"></param>
        public ReverseProxyExecutingContext(HttpRequest incomingRequest, HttpRequestMessage proxyRequest)
        {
            IncomingRequest = incomingRequest;
            ProxyRequest = proxyRequest;
        }

        /// <summary>
        /// The incoming request.
        /// </summary>
        public HttpRequest IncomingRequest { get; }

        /// <summary>
        /// The proxy request cloned from <see cref="IncomingRequest"/>
        /// </summary>
        public HttpRequestMessage ProxyRequest { get; }
    } 
}
