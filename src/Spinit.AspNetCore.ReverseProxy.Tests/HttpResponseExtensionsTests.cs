using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Spinit.AspNetCore.ReverseProxy.Tests.Infrastructure;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.HttpResponseExtensions
{
    public class AssignAsync
    {
        [Fact]
        public async Task ShouldReturn500IfSourceIsNull()
        {
            var target = CreateHttpResponse();
            HttpResponseMessage source = null;
            await target.AssignAsync(source);
            Assert.Equal((int)HttpStatusCode.InternalServerError, target.StatusCode);
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetRequestHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldAddSourceHeader(string headerName)
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            Assert.True(source.Headers.TryAddWithoutValidation(headerName, Guid.NewGuid().ToString()));
            await target.AssignAsync(source);
            Assert.True(target.Headers.ContainsKey(headerName));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetRequestHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldAddSourceHeaderValue(string headerName)
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            var headerValue = Guid.NewGuid().ToString();
            Assert.True(source.Headers.TryAddWithoutValidation(headerName, headerValue));
            await target.AssignAsync(source);
            Assert.Equal(headerValue, target.Headers[headerName]);
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetContentHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldAddSourceContentHeader(string headerName)
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            Assert.True(source.Content.Headers.TryAddWithoutValidation(headerName, Guid.NewGuid().ToString()));
            await target.AssignAsync(source);
            Assert.True(target.Headers.ContainsKey(headerName));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetContentHeaderTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldAddSourceContentHeaderValue(string headerName)
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            source.Content.Headers.Clear();
            var headerValue = Guid.NewGuid().ToString();
            Assert.True(source.Content.Headers.TryAddWithoutValidation(headerName, headerValue));
            await target.AssignAsync(source);
            Assert.Equal(headerValue, target.Headers[headerName]);
        }

        [Fact]
        public async Task ShouldRemoveTransferEncoding()
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            source.Headers.Add("transfer-encoding", "");
            await target.AssignAsync(source);
            Assert.DoesNotContain(target.Headers, x => x.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase));
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetHttpStatusCodeTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldSetStatusCode(int statusCode)
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage((HttpStatusCode)statusCode)
            {
                Content = new StringContent("")
            };
            await target.AssignAsync(source);
            Assert.Equal(statusCode, target.StatusCode);
        }

        [Fact]
        public async Task ShouldNotCopyContentIf204()
        {
            var target = CreateHttpResponse();
            var source = new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                Content = new StringContent("Some data")
            };
            await target.AssignAsync(source);
            Assert.Equal(0, target.Body.Length);
        }

        [Theory]
        [MemberData(nameof(HttpTestingUtils.GetHttpStatusCodeTheories), MemberType = typeof(HttpTestingUtils))]
        public async Task ShouldCopyContentIfNot204(int statusCode)
        {
            if (statusCode == 204)
                return;

            var target = CreateHttpResponse();
            var source = new HttpResponseMessage((HttpStatusCode)statusCode)
            {
                Content = new StringContent("Some data")
            };
            await target.AssignAsync(source);
            Assert.NotEqual(0, target.Body.Length);
        }

        private static DefaultHttpResponse CreateHttpResponse()
        {
            return new DefaultHttpResponse(new DefaultHttpContext())
            {
                Body = new MemoryStream()
            };
        }
    }
}
