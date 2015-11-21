﻿using System.Threading.Tasks;
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
            Requires.ArgumentNotNull(halClient, nameof(halClient));

            _halClient = halClient;
        }

        public async Task<User> GetAsync()
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<User>(root.UserLink).ConfigureAwait(_halClient);
        }

        public async Task<User> UpdateAsync(UserUpdate userUpdate)
        {
            var user = await GetAsync().ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<User>(user.UpdateLink, userUpdate).ConfigureAwait(_halClient);
        }
    }
}