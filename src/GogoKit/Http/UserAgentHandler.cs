using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Http
{
    public class UserAgentHandler : DelegatingHandler
    {
        private IReadOnlyList<ProductInfoHeaderValue> _userAgentHeaderValues;

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
                new ProductInfoHeaderValue(string.Format("({0}; {1} {2})",
                                                         CultureInfo.CurrentCulture.Name,
                                                         "GogoKit",
                                                         AssemblyVersionInformation.Version))
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
