using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Spinit.AspNetCore.ReverseProxy
{
    public class ReverseProxyExecutingContext
    {
        public ReverseProxyExecutingContext(HttpRequest incomingRequest, HttpRequestMessage proxyRequest)
        {
            this.IncomingRequest = incomingRequest;
            this.ProxyRequest = proxyRequest;
        }

        public HttpRequest IncomingRequest { get; }

        public HttpRequestMessage ProxyRequest { get; }
    } 
}
