using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.Filters
{
    public class RemoveConnectionHeaderFilterTests
    {
        [Fact]
        public async Task AssertConnectionRemoved()
        {
            var filter = new RemoveConnectionHeaderFilter();
            var proxyRequest = new HttpRequestMessage();
            proxyRequest.Headers.Connection.Add("keep-alive");
            var context = new ReverseProxyExecutingContext(null, proxyRequest);
            await filter.OnExecutingAsync(context);
            Assert.Empty(proxyRequest.Headers.Connection);
        }
    }
}
