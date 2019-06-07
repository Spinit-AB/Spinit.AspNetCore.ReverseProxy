using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    public class RemoveHostHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {
            context.ProxyRequest.Headers.Host = null;
            return Task.CompletedTask;
        }
    }
}
