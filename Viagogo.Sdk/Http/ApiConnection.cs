using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public class ApiConnection : IApiConnection
    {
        private const string HalJsonMediaType = "application/hal+json";

        private readonly IConnection _connection;

        public ApiConnection(IConnection connection)
        {
            Requires.ArgumentNotNull(connection, "connection");

            _connection = connection;
        }

        public IConnection Connection
        {
            get { return _connection; }
        }

        public Task<T> PostAsync<T>(Uri uri, object data)
        {
            return SendRequestAsync<T>(uri, HttpMethod.Post, data);
        }

        private async Task<T> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            object data)
        {
            var response = await _connection.SendRequestAsync<T>(
                                    uri,
                                    method,
                                    HalJsonMediaType,
                                    data,
                                    HalJsonMediaType);
            return response.BodyAsObject;
        }
    }
}
