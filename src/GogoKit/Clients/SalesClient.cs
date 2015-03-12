using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class SalesClient : ISalesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _connection;

        public SalesClient(IUserClient userClient,
            IHypermediaConnection connection)
        {
            _userClient = userClient;
            _connection = connection;
        }

        public async Task<PagedResource<Sale>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Sale>>(
                user.Links["user:sales"],
                null).ConfigureAwait(_connection);
        }
    }
}