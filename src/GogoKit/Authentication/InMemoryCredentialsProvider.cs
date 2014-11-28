using System.Threading.Tasks;

namespace GogoKit.Authentication
{
    public class InMemoryCredentialsProvider : ICredentialsProvider
    {
        private readonly ICredentials _credentials;

        public InMemoryCredentialsProvider(ICredentials credentials)
        {
            Requires.ArgumentNotNull(credentials, "credentials");

            _credentials = credentials;
        }

        public Task<ICredentials> GetCredentialsAsync()
        {
            return Task.FromResult(_credentials);
        }
    }
}