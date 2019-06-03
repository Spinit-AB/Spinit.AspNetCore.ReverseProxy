using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class AddXForwardedForHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {            
            context.ProxyRequest.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, context.IncomingRequest.HttpContext.Connection.RemoteIpAddress.ToString());
            return Task.CompletedTask;
        }
    }
}
