using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Defines the methods that are used in a reverse proxy filter.
    /// </summary>
    public interface IReverseProxyFilter
    {
        /// <summary>
        /// Called before a proxy request is sent.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnExecutingAsync(ReverseProxyExecutingContext context);
        //public Task OnExecutedAsync(ReverseProxyExecutedContext context);
    }
}
