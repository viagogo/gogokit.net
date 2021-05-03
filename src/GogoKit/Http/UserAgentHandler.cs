using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Http
{
    public class UserAgentHandler : DelegatingHandler
    {
        private static readonly ProductInfoHeaderValue GogoKitVersionHeaderValue =
            new ProductInfoHeaderValue(
                "GogoKit",
                FileVersionInfo.GetVersionInfo(typeof(UserAgentHandler).Assembly.Location).ProductVersion);

        private readonly IReadOnlyList<ProductInfoHeaderValue> _userAgentHeaderValues;

        public UserAgentHandler(ProductHeaderValue product)
        {
            Requires.ArgumentNotNull(product, nameof(product));

            _userAgentHeaderValues = GetUserAgentHeaderValues(product);
        }

        private IReadOnlyList<ProductInfoHeaderValue> GetUserAgentHeaderValues(ProductHeaderValue product)
        {
            return new List<ProductInfoHeaderValue>
            {
                new ProductInfoHeaderValue(product),
                GogoKitVersionHeaderValue
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            foreach (var product in _userAgentHeaderValues)
            {
                request.Headers.UserAgent.Add(product);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
