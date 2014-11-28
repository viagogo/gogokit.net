using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class ApiRootClient : IApiRootClient
    {
        private readonly Uri _apiRootUrl;
        private readonly IConnection _connection;

        public ApiRootClient(Uri viagogoApiUrl, IConnection connection)
        {
            Requires.ArgumentNotNull(viagogoApiUrl, "viagogoApiUrl");
            Requires.ArgumentNotNull(connection, "connection");

            _apiRootUrl = new Uri(viagogoApiUrl, "/v2");
            _connection = connection;
        }

        public async Task<ApiRoot> GetAsync()
        {
            var response = await _connection.SendRequestAsync<ApiRoot>(
                                    _apiRootUrl,
                                    HttpMethod.Get,
                                    "application/hal+json",
                                    null,
                                    null);
            return response.BodyAsObject;
        }
    }
}