using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Spinit.AspNetCore.ReverseProxy.Tests.Infrastructure
{
    internal static class HttpTestingUtils
    {
        public static TheoryData<int> GetHttpStatusCodeTheories()
        {
            var statusCodes = new List<int>();
            foreach (int value in Enum.GetValues(typeof(HttpStatusCode)))
                statusCodes.Add(value);
            statusCodes = statusCodes.Distinct().ToList();

            var result = new TheoryData<int>();
            foreach (var statusCode in statusCodes)
            {
                result.Add(statusCode);
            }
            return result;
        }

        public static TheoryData<string> GetHttpMethodTheories()
        {
            var result = new TheoryData<string>();
            foreach (var value in GetHttpMethods())
                result.Add(value);
            return result;
        }

        public static TheoryData<string> GetRequestHeaderTheories()
        {
            var result = new TheoryData<string>();
            foreach (var header in GetRequestHeaders())
            {
                if (header.Equals(HeaderNames.TransferEncoding, StringComparison.OrdinalIgnoreCase))
                    continue;
                result.Add(header);
            }
            return result;
        }

        public static TheoryData<string> GetContentHeaderTheories()
        {
            return GetContentHeaderTheories(null);
        }

        public static TheoryData<string> GetContentHeaderTheories(string excludePattern)
        {
            var result = new TheoryData<string>();
            foreach (var header in GetContentHeaders())
            {
                if (string.IsNullOrEmpty(excludePattern) || !Regex.IsMatch(header, excludePattern))
                    result.Add(header);
            }
            return result;
        }

        public static IEnumerable<string> GetRequestHeaders()
        {
            var httpResponseMessage = new HttpRequestMessage();
            return GetKnownHeaders().Where(x => httpResponseMessage.Headers.TryAddWithoutValidation(x, Guid.NewGuid().ToString())).ToList();
        }

        public static IEnumerable<string> GetContentHeaders()
        {
            var content = new StringContent("");
            return GetKnownHeaders().Where(x => x.StartsWith("Content-") && content.Headers.TryAddWithoutValidation(x, Guid.NewGuid().ToString())).ToList();
        }

        public static IEnumerable<string> GetKnownHeaders()
        {
            return typeof(HeaderNames).GetFields().Select(x => x.GetValue(null).ToString()).Where(x => !x.StartsWith(":")).ToArray();
        }

        public static IEnumerable<string> GetHttpMethods()
        {
            return typeof(HttpMethods).GetFields().Select(x => x.GetValue(null).ToString());
        }
    }
}
