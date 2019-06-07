using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IReverseProxy))]
        [InlineData(typeof(IHttpClientFactory))]
        public void ServicesShouldContain(Type serviceType)
        {
            var services = new ServiceCollection();
            services.AddReverseProxy();
            Assert.Contains(services, x => x.ServiceType == serviceType);
        }
    }
}
