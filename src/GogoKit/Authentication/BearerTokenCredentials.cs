namespace GogoKit.Authentication
{
    public class BearerTokenCredentials : ICredentials
    {
        private readonly string _accessToken;

        public BearerTokenCredentials(string accessToken)
        {
            _accessToken = accessToken;
        }

        public string AuthorizationHeader
        {
            get { return string.Format("Bearer {0}", _accessToken); }
        }
    }
}
