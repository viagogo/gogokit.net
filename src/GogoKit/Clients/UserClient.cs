using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.RequestModels;
using GogoKit.Resources;

namespace GogoKit.Clients
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

        public async Task<User> PatchAsync(UserRequest userRequest)
        {
            var root = await _rootClient.GetAsync();
            var user = await _connection.GetAsync<User>(root.Links["viagogo:user"], null);
            return await _connection.PatchAsync<User>(user.Links["user:update"], null, userRequest);
        }
    }
}