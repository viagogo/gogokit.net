using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class UserClient : IUserClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;

        public UserClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            Requires.ArgumentNotNull(rootClient, "rootClient");
            Requires.ArgumentNotNull(connection, "connection");

            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<User> GetAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<User>(root.Links["viagogo:user"], null).ConfigureAwait(_connection);
        }

        public async Task<User> UpdateAsync(UserUpdate userUpdate)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            var user = await _connection.GetAsync<User>(root.Links["viagogo:user"], null).ConfigureAwait(_connection);
            return await _connection.PatchAsync<User>(user.Links["user:update"], null, userUpdate).ConfigureAwait(_connection);
        }
    }
}