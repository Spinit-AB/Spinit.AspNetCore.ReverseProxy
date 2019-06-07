using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Spinit.AspNetCore.ReverseProxy.Tests.Infrastructure;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.HttpRequestExtensions
{
    public class Assign
    {
        [Fact]
        public void ShouldSetRequestUri()
        {
            var target = new HttpRequestMessage();
            var uri = "https://localhost:8080/a/path/to/resource?param1=true&param2=false";
            var source = CreateHttpRequest(uri);
            target.Assign(source);
            Assert.Equal(uri, target.RequestUri.ToString());
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetHttpMethodTheories), MemberType = typeof(HttpTestingUtils))]
        public void ShouldSetRequestMethod(string httpMethod)
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            source.Method = httpMethod;
            target.Assign(source);
            Assert.Equal(httpMethod, target.Method.Method);
        }

        [Fact]
        public async Task ShouldSetStreamContent()
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            var content = "Come content";
            using (var writer = new StreamWriter(source.Body, encoding: Encoding.UTF8, bufferSize: 8192, leaveOpen: true))
            {
                writer.Write(content);
            }
            source.Body.Position = 0;
            source.ContentLength = source.Body.Length;
            target.Assign(source);
            var actualContent = await target.Content.ReadAsStringAsync();
            Assert.Equal(content, actualContent);
        }

        [Fact]
        public void ShouldSetFormContent()
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            var formValues = new Dictionary<string, StringValues>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            };

            source.Form = new FormCollection(formValues);
            source.ContentLength = 1; // how to calculate Content-Length?
            target.Assign(source);
            var actualContent = target.Content as FormUrlEncodedContent;
            Assert.NotNull(actualContent);
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetRequestHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public void ShouldSetHeader(string headerName)
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            if (headerName != HeaderNames.Host)
                source.Headers.Add(headerName, Guid.NewGuid().ToString());
            target.Assign(source);
            Assert.True(target.Headers.Contains(headerName));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetRequestHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public void ShouldSetHeaderValue(string headerName)
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            if (headerName != HeaderNames.Host)
                source.Headers.Add(headerName, Guid.NewGuid().ToString());
            target.Assign(source);
            Assert.Equal(source.Headers[headerName], target.Headers.GetValues(headerName));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetContentHeaderTheories), "^Content-Security.*$", MemberType = typeof(HttpTestingUtils))]
        public void ShouldSetContentHeader(string headerName)
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            source.ContentLength = 1;
            if (headerName != HeaderNames.ContentLength)
                source.Headers.Add(headerName, Guid.NewGuid().ToString());
            target.Assign(source);
            Assert.True(target.Content.Headers.Contains(headerName));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetContentHeaderTheories), "^Content-Security.*$", MemberType = typeof(HttpTestingUtils))]
        public void ShouldSetContentHeaderValue(string headerName)
        {
            var target = new HttpRequestMessage();
            var source = CreateHttpRequest();
            source.ContentLength = 1;
            if (headerName != HeaderNames.ContentLength)
                source.Headers.Add(headerName, Guid.NewGuid().ToString());
            target.Assign(source);
            Assert.Equal(source.Headers[headerName], target.Content.Headers.GetValues(headerName));
        }

        private static DefaultHttpRequest CreateHttpRequest(string uri = "http://localhost")
        {
            UriHelper.FromAbsolute(uri, out var scheme, out var host, out var path, out var query, out _);

            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = HttpMethods.Get,
                Scheme = scheme,
                Host = host,
                Path = path,
                QueryString = query,
                Body = new MemoryStream()
            };
        }
    }


}
