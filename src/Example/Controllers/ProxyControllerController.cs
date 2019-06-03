using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spinit.AspNetCore.ReverseProxy;

namespace ReverseProxyExample.Controllers
{
    /// <summary>
    /// Using the reverse proxy in a controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyControllerController : ControllerBase
    {
        private readonly IReverseProxy _reverseProxy;

        public ProxyControllerController(IReverseProxy reverseProxy)
        {
            _reverseProxy = reverseProxy;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var proxyUri = new Uri("/api/protected", UriKind.Relative);
            var result = await _reverseProxy.ExecuteAsync(Request, proxyUri).ConfigureAwait(false);
            if (result == null)
                throw new Exception("Could not execute reverse proxy");
            return new ResponseResult(result);

            // TODO: return new ResponseResult(_reverseProxy.ExecuteAsync(Request, proxyUri));
        }
    }
}
