using Viagogo.Sdk.Authentication;

namespace Viagogo.Sdk.Tests.Fakes
{
    public class FakeCredentials : ICredentials
    {
        private readonly string _authHeader;

        public FakeCredentials(string authHeader = "Fake 123456")
        {
            _authHeader = authHeader;
        }

        public string AuthorizationHeader
        {
            get { return _authHeader; }
        }
    }
}
