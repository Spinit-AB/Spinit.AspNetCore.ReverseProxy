using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Spinit.AspNetCore.ReverseProxy
{
    public class ResponseResult : ActionResult
    {
        private readonly HttpResponseMessage _response;

        public ResponseResult(HttpResponseMessage response)
        {
            _response = response;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return context.HttpContext.Response.AssignAsync(_response);
        }
    }
}
