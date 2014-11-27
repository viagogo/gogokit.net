using System.Threading.Tasks;
using Viagogo.Sdk.Http;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Clients
{
    public class UserClient : IUserClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IApiConnection _connection;

        public UserClient(IApiRootClient rootClient, IApiConnection connection)
        {
            Requires.ArgumentNotNull(rootClient, "rootClient");
            Requires.ArgumentNotNull(connection, "connection");

            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<User> GetAsync()
        {
            var root = await _rootClient.GetAsync();
            return await _connection.GetAsync<User>(root.Links["viagogo:user"], null);
        }
    }
}