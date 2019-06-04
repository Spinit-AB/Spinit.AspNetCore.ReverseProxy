using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Represents a proxy that can relay a request to another url.
    /// </summary>
    public interface IReverseProxy
    {
        /// <summary>
        /// Executes a proxy request, eg relays it to another provided url.
        /// </summary>
        /// <param name="incomingRequest">The incoming request, normally from Mvc.</param>
        /// <param name="proxyUri">The <see cref="Uri"/> to relay the <paramref name="incomingRequest"/> to.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> ExecuteAsync(HttpRequest incomingRequest, Uri proxyUri);
    }
}
