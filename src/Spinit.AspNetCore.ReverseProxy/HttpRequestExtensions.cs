using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Spinit.AspNetCore.ReverseProxy
{
    internal static class HttpRequestExtensions
    {
        internal static HttpRequestMessage Assign(this HttpRequestMessage target, HttpRequest source)
        {
            target.RequestUri = new Uri(UriHelper.GetEncodedUrl(source));
            target.Method = new HttpMethod(source.Method);
            target.AssignContent(source);
            target.AssignHeaders(source);
            return target;
        }

        private static void AssignContent(this HttpRequestMessage proxyRequest, HttpRequest source)
        {
            if (source.ContentLength.GetValueOrDefault(0) == 0)
                return;

            if (source.HasFormContentType)
            {
                var formFields = source.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                proxyRequest.Content = new FormUrlEncodedContent(formFields);
            }
            else
            {
                proxyRequest.Content = new StreamContent(source.Body);
            }
        }

        internal static void AssignHeaders(this HttpRequestMessage proxyRequest, HttpRequest source)
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
