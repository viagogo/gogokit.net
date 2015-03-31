using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;
using HalKit;

namespace GogoKit.Clients
{
    public class UserClient : IUserClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHalClient _halClient;

        public UserClient(IApiRootClient rootClient, IHalClient halClient)
        {
            Requires.ArgumentNotNull(rootClient, "rootClient");
            Requires.ArgumentNotNull(halClient, "halClient");

            _rootClient = rootClient;
            _halClient = halClient;
        }

        public async Task<User> GetAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_halClient);
            return root.AuthenticatedUser;
        }

        public async Task<User> UpdateAsync(UserUpdate userUpdate)
        {
            var user = await GetAsync().ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<User>(user.Links["user:update"], userUpdate).ConfigureAwait(_halClient);
        }
    }
}