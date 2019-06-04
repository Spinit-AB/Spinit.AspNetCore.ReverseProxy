using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.Filters.ReverseProxyFilterExtensions
{
    public class Add
    {
        [Fact]
        public void AddShouldAddANewInstance()
        {
            var filters = new List<IReverseProxyFilter>();
            filters.Add<TestFilter>();
            Assert.Single(filters);
        }

        public class TestFilter : IReverseProxyFilter
        {
            public Task OnExecutingAsync(ReverseProxyExecutingContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
