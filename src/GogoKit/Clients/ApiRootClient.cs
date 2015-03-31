using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Resources;
using HalKit.Http;

namespace GogoKit.Clients
{
    public class ApiRootClient : IApiRootClient
    {
        private readonly Uri _apiRootUrl;
        private readonly IHttpConnection _connection;

        public ApiRootClient(IHttpConnection connection)
        {
            Requires.ArgumentNotNull(connection, "connection");

            _apiRootUrl = null;//new Uri(connection.Configuration.ViagogoApiUrl, "/v2?embed=user");
            _connection = connection;
        }

        public async Task<ApiRoot> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}