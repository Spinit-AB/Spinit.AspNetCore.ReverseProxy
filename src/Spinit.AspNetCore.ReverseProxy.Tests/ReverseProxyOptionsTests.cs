using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests
{
    public class ReverseProxyOptionsTests
    {

        [Theory]
        [InlineData(nameof(RemoveConnectionHeaderFilter))]
        [InlineData(nameof(RemoveHostHeaderFilter))]
        public void ShouldByDefaultContainFilter(string filterTypeName)
        {
            var proxy = new ReverseProxyOptions();
            Assert.Contains(proxy.Filters, x => x.GetType().Name == filterTypeName);
        }
    }
}
