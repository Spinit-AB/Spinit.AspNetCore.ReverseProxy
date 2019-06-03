using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class AddXForwardedProtoHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {            
            context.ProxyRequest.Headers.Add(ForwardedHeadersDefaults.XForwardedProtoHeaderName, context.IncomingRequest.Protocol);
            return Task.CompletedTask;
        }
    }
}
