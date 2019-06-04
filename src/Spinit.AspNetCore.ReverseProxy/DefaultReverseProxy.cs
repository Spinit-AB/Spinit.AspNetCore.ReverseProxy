using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Default implementation of a <see cref="IReverseProxy"/> that uses a <see cref="HttpClient"/> to call the proxied url.
    /// </summary>
    public class DefaultReverseProxy : IReverseProxy
    {
        private readonly HttpClient _httpClient;
        private readonly ReverseProxyOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseProxy"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to call the proxied url.</param>
        /// <param name="options"></param>
        public DefaultReverseProxy(HttpClient httpClient, IOptions<ReverseProxyOptions> options)
            : this(httpClient, options.Value)
        { }

        internal DefaultReverseProxy(HttpClient httpClient, ReverseProxyOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        /// <summary>
        /// Executes a proxy request, eg relays it to another provided url.
        /// </summary>
        /// <param name="incomingRequest">The incoming request, normally from Mvc.</param>
        /// <param name="proxyUri">The <see cref="Uri"/> to relay the <paramref name="incomingRequest"/> to.</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequest incomingRequest, Uri proxyUri)
        {
            var proxyRequest = CreateProxyRequest(incomingRequest, proxyUri);
            try
            {
                await _options.Filters.OnExecutingAsync(new ReverseProxyExecutingContext(incomingRequest, proxyRequest)).ConfigureAwait(false);
                return await _httpClient.SendAsync(proxyRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                // TODO: add Filters.OnExecutedAsync(...)
            }
            catch (HttpRequestException e)
            {
                //_logger.LogError(new EventId(0), e, "Api call to {0} failed", serviceUrl);
                throw;
            }
        }

        internal static HttpRequestMessage CreateProxyRequest(HttpRequest source, Uri proxyUri)
        {
            proxyUri = BuildProxyUri(source, proxyUri);

            var proxyRequest = new HttpRequestMessage(new HttpMethod(source.Method), proxyUri);
            TrySetProxyRequestBody(proxyRequest, source);
            TrySetProxyRequestHeaders(proxyRequest, source);
            return proxyRequest;
        }

        internal static Uri BuildProxyUri(HttpRequest source, Uri proxyUri)
        {
            if (!proxyUri.IsAbsoluteUri)
                proxyUri = new Uri(new Uri(source.GetEncodedUrl()), proxyUri);

            return new Uri(proxyUri, source.QueryString.ToUriComponent());
        }

        internal static void TrySetProxyRequestBody(HttpRequestMessage proxyRequest, HttpRequest source)
        {
            if (source.Body == null)
                return;

            /*
            if (source.HasFormContentType)
            {
                var formFields = source.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                proxyRequest.Content = new FormUrlEncodedContent(formFields);
            }
            else
            */
            proxyRequest.Content = new StreamContent(source.Body);
        }

        internal static void TrySetProxyRequestHeaders(HttpRequestMessage proxyRequest, HttpRequest source)
        {
            foreach (var header in source.Headers)
            {
                if (!proxyRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    proxyRequest.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }
    }
}
