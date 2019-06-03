using System.Collections.Generic;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    public class ReverseProxyOptions
    {
        public ReverseProxyOptions()
        {
            Filters = new List<IReverseProxyFilter>();
        }

        public List<IReverseProxyFilter> Filters { get; }

        // not released yet...
        internal ReverseProxyOptions AddForwardedHeaders(ForwardedHeaders forwardedHeaders)
        {
            if (forwardedHeaders.HasFlag(ForwardedHeaders.XForwardedFor))
                Filters.Add<AddXForwardedForHeaderFilter>();
            if (forwardedHeaders.HasFlag(ForwardedHeaders.XForwardedHost))
                Filters.Add<AddXForwardedHostHeaderFilter>();
            if (forwardedHeaders.HasFlag(ForwardedHeaders.XForwardedProto))
                Filters.Add<AddXForwardedProtoHeaderFilter>();
            return this;
        }
    }
}
