using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

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

            target.StatusCode = (int)source.StatusCode;

            foreach (var header in source.Headers)
            {
                target.Headers[header.Key] = header.Value.ToArray();
            }
            foreach (var header in source.Content.Headers)
            {
                target.Headers[header.Key] = header.Value.ToArray();
            }

            // HttpClient.SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            target.Headers.Remove(HeaderNames.TransferEncoding);

            if (source.StatusCode != HttpStatusCode.NoContent)
                await source.Content.CopyToAsync(target.Body).ConfigureAwait(false);
            return target;
        }
    }
}
