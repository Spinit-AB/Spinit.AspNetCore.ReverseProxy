using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spinit.AspNetCore.ReverseProxy
{
    /// <summary>
    /// Extensionmethods for <see cref="IReverseProxyFilter"/>
    /// </summary>
    public static class ReverseProxyFilterExtensions
    {
        /// <summary>
        /// Adds a <see cref="IReverseProxyFilter"/> to the list.
        /// </summary>
        /// <typeparam name="TFilter">The filter type to add</typeparam>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IList<IReverseProxyFilter> Add<TFilter>(this IList<IReverseProxyFilter> filters)
            where TFilter : IReverseProxyFilter, new()
        {
            var filter = Activator.CreateInstance<TFilter>();
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
