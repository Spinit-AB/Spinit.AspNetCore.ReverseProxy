using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Spinit.AspNetCore.ReverseProxy
{
    public interface IReverseProxy
    {
        /// <summary>
        /// Execute the proxy
        /// </summary>
        /// <param name="incomingRequest"></param>
        /// <param name="proxyUri">The <see cref="Uri"/> to relay the <paramref name="incomingRequest"/> to.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> ExecuteAsync(HttpRequest incomingRequest, Uri proxyUri);
    }
}
