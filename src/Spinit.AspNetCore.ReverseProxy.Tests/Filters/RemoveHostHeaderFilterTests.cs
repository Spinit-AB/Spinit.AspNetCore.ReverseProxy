using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.Filters
{
    public class RemoveHostHeaderFilterTests
    {
        [Fact]
        public async Task AssertHostRemoved()
        {
            var filter = new RemoveHostHeaderFilter();
            var proxyRequest = new HttpRequestMessage();
            proxyRequest.Headers.Host = "localhost";
            var context = new ReverseProxyExecutingContext(null, proxyRequest);
            await filter.OnExecutingAsync(context);
            Assert.Null(proxyRequest.Headers.Host);
        }
    }
}
