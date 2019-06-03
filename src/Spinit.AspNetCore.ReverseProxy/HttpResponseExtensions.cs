using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal static class HttpResponseExtensions
    {
        internal static async Task<HttpResponse> AssignAsync(this HttpResponse target, HttpResponseMessage source)
        {
            if (source == null)
            {
                target.StatusCode = (int)HttpStatusCode.InternalServerError;
                return target;
            }
            foreach (var header in source.Headers)
            {
                target.Headers[header.Key] = header.Value.ToArray();
            }
            foreach (var header in source.Content.Headers)
            {
                target.Headers[header.Key] = header.Value.ToArray();
            }
            target.Headers.Remove("transfer-encoding");
            target.StatusCode = (int)source.StatusCode;

            if (source.StatusCode != HttpStatusCode.NoContent)
                await source.Content.CopyToAsync(target.Body).ConfigureAwait(false);
            return target;
        }
    }
}
