using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Http
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly AuthenticationHeaderValue _authorizationHeader;

        public BasicAuthenticationHandler(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;

            var loginAndPassword = string.Format("{0}:{1}", _clientId, _clientSecret);
            var headerText = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(loginAndPassword)));
            _authorizationHeader = AuthenticationHeaderValue.Parse(headerText);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = _authorizationHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}