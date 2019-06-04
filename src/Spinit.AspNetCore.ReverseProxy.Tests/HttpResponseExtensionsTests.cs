using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Net.Http.Headers;
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
        [MemberData(nameof(GetRequestHeaderTheories))]
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
        [MemberData(nameof(GetRequestHeaderTheories))]
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
        [MemberData(nameof(GetContentHeaderTheories))]
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
        [MemberData(nameof(GetContentHeaderTheories))]
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
        [MemberData(nameof(GetHttpStatusCodeTheories))]
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
        [MemberData(nameof(GetHttpStatusCodeTheories))]
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

        public static TheoryData<string> GetRequestHeaderTheories()
        {
            var result = new TheoryData<string>();
            foreach (var header in GetRequestHeaders())
            {
                if (header.Equals(HeaderNames.TransferEncoding, StringComparison.OrdinalIgnoreCase))
                    continue;
                result.Add(header);
            }
            return result;
        }

        public static TheoryData<string> GetContentHeaderTheories()
        {
            var result = new TheoryData<string>();
            foreach (var header in GetContentHeaders())
            {
                result.Add(header);
            }
            return result;
        }

        public static IEnumerable<string> GetRequestHeaders()
        {
            var httpResponseMessage = new HttpRequestMessage();
            return GetKnownHeaders().Where(x => httpResponseMessage.Headers.TryAddWithoutValidation(x, Guid.NewGuid().ToString())).ToList();
        }

        public static IEnumerable<string> GetContentHeaders()
        {
            var content = new StringContent("");
            return GetKnownHeaders().Where(x => content.Headers.TryAddWithoutValidation(x, Guid.NewGuid().ToString())).ToList();
        }

        public static IEnumerable<string> GetKnownHeaders()
        {
            return typeof(HeaderNames).GetFields().Select(x => x.GetValue(null).ToString()).Where(x => !x.StartsWith(":")).ToArray();
        }

        public static TheoryData<int> GetHttpStatusCodeTheories()
        {
            var statusCodes = new List<int>();
            foreach (int value in Enum.GetValues(typeof(HttpStatusCode)))
                statusCodes.Add(value);
            statusCodes = statusCodes.Distinct().ToList();

            var result = new TheoryData<int>();
            foreach (var statusCode in statusCodes)
            {
                result.Add(statusCode);
            }
            return result;
        }
    }
}
