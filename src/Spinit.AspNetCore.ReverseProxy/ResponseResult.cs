using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// A <see cref="IActionResult"/> implementation that wraps a <see cref="HttpResponseMessage"/>
    /// </summary>
    public class ResponseResult : ActionResult
    {
        private readonly HttpResponseMessage _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseResult"/> class.
        /// </summary>
        /// <param name="response">The <see cref="HttpRequestMessage"/> that should be used as response.</param>
        public ResponseResult(HttpResponseMessage response)
        {
            _response = response;
        }

        /// <summary>
        /// Assigns the <see cref="HttpContext.Response"/> from  the supplied <see cref="HttpResponseMessage"/>. 
        /// This method is called by MVC to process the result of an action method. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            return context.HttpContext.Response.AssignAsync(_response);
        }
    }
}
