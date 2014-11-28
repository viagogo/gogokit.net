using System;
using System.Text;

namespace GogoKit.Authentication
{
    public class BasicCredentials : ICredentials
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _authorizationHeader;

        public BasicCredentials(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _authorizationHeader = null;
        }

        public string AuthorizationHeader
        {
            get
            {
                if (_authorizationHeader == null)
                {
                    var loginAndPassword = string.Format("{0}:{1}", _clientId, _clientSecret);
                    _authorizationHeader = string.Format(
                                            "Basic {0}",
                                            Convert.ToBase64String(Encoding.UTF8.GetBytes(loginAndPassword)));
                }

                return _authorizationHeader;
            }
        }
    }
}