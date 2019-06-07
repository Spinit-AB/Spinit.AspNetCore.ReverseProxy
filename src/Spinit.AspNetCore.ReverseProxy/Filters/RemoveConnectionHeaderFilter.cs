using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    public class RemoveConnectionHeaderFilter : IReverseProxyFilter
    {
        public Task OnExecutingAsync(ReverseProxyExecutingContext context)
        {
            context.ProxyRequest.Headers.Connection?.Clear();
            return Task.CompletedTask;
        }
    }
}
