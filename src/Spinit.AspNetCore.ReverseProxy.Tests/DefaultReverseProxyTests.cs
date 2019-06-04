namespace Spinit.AspNetCore.ReverseProxy.Tests.DefaultReverseProxy
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Spinit.AspNetCore.ReverseProxy;
    using Xunit;

    public class BuildProxyUri
    {
        [Theory]
        [InlineData("http://localhost/api/search", "http://localhost:5000", "http://localhost:5000/")]
        [InlineData("http://localhost/api/search?site=1", "http://localhost:5000/api", "http://localhost:5000/api?site=1")]
        [InlineData("http://localhost/api/search?site=1", "/protected/search", "http://localhost/protected/search?site=1")]
        public void ShouldReturnExpectedValue(string incomingUri, string proxyUri, string expectedUri)
        {
            var httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            var uri = new Uri(incomingUri, UriKind.Absolute);
            httpRequest.Scheme = uri.Scheme;
            httpRequest.Host = new HostString(uri.Host, uri.Port);
            httpRequest.Path = uri.AbsolutePath;
            httpRequest.QueryString = new QueryString(uri.Query);
            var result = DefaultReverseProxy.BuildProxyUri(httpRequest, new Uri(proxyUri, UriKind.RelativeOrAbsolute));
            Assert.Equal(expectedUri, result.ToString());
        }
    }
}
