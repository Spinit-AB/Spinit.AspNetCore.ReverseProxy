﻿using System.Collections.Generic;
using Microsoft.AspNetCore.HttpOverrides;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Provides programmatic configuration for the <see cref="IReverseProxy"/>.
    /// </summary>
    public class ReverseProxyOptions
    {
        internal static readonly IEnumerable<IReverseProxyFilter> DefaultReverseProxyFilters = new IReverseProxyFilter[]
        {
            new RemoveConnectionHeaderFilter(),
            new RemoveHostHeaderFilter(),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseProxyOptions"/> class with the default filters.
        /// </summary>
        public ReverseProxyOptions()
        {
            Filters = new List<IReverseProxyFilter>(DefaultReverseProxyFilters);
        }

        /// <summary>
        /// Gets a collection of <see cref="IReverseProxyFilter"/> that apply to all routes.
        /// </summary>
        public IList<IReverseProxyFilter> Filters { get; }

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
