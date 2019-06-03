using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    public static class ReverseProxyFilterExtensions
    {
        public static IList<IReverseProxyFilter> Add<TFilter>(this IList<IReverseProxyFilter> filters)
            where TFilter : IReverseProxyFilter, new()
        {
            var filter = Activator.CreateInstance<TFilter>();
            filters.Add(filter);
            return filters;
        }

        public static IList<IReverseProxyFilter> Add<TFilter>(this IList<IReverseProxyFilter> filters, TFilter filter)
            where TFilter : IReverseProxyFilter
        {
            filters.Add(filter);
            return filters;
        }

        internal static async Task OnExecutingAsync(this IList<IReverseProxyFilter> filters, ReverseProxyExecutingContext context)
        {
            if (filters == null)
                return;
            foreach (var filter in filters)
            {
                await filter.OnExecutingAsync(context).ConfigureAwait(false);
            }
        }
    }
}
