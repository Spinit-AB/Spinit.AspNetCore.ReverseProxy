using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    public interface IReverseProxyFilter
    {
        Task OnExecutingAsync(ReverseProxyExecutingContext context);
        //public Task OnExecutedAsync(ReverseProxyExecutedContext context);
    }
}
