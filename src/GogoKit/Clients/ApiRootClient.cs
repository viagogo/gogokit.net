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
        private readonly IHttpConnection _connection;

        public ApiRootClient(Uri viagogoApiUrl, IHttpConnection connection)
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
                                    null).ConfigureAwait(false);
            return response.BodyAsObject;
        }
    }
}