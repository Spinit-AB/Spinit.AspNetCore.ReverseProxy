using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal class AddXForwardedForHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {
            // TODO: add incoming ForwardedHeadersDefaults.XForwardedForHeaderName headers like:
            /*
            var values = context.IncomingRequest.Headers.Where(x => x.Key.Equals(ForwardedHeadersDefaults.XForwardedForHeaderName)).SelectMany(x => x.Value).ToList();
            values.Add(context.IncomingRequest.HttpContext.Connection.RemoteIpAddress.ToString());
            context.ProxyRequest.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, values);
            */
            context.ProxyRequest.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, context.IncomingRequest.HttpContext.Connection.RemoteIpAddress.ToString());
            return Task.CompletedTask;
        }
    }
}
