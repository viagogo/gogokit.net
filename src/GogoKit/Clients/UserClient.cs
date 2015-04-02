using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit;

namespace GogoKit.Clients
{
    public class UserClient : IUserClient
    {
        private readonly IHalClient _halClient;

        public UserClient(IHalClient halClient)
        {
            Requires.ArgumentNotNull(halClient, "halClient");

            _halClient = halClient;
        }

        public async Task<User> GetAsync()
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<User>(root.Links["viagogo:user"]).ConfigureAwait(_halClient);
        }

        public async Task<User> UpdateAsync(UserUpdate userUpdate)
        {
            var user = await GetAsync().ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<User>(user.Links["user:update"], userUpdate).ConfigureAwait(_halClient);
        }
    }
}