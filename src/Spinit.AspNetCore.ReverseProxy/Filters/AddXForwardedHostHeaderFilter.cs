using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class AddXForwardedHostHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {
            context.ProxyRequest.Headers.Add(ForwardedHeadersDefaults.XForwardedHostHeaderName, context.IncomingRequest.Host.ToUriComponent());
            return Task.CompletedTask;
        }
    }
}
