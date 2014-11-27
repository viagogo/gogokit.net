using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viagogo.Sdk.Models;

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

        public Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNull(link, "link");

            // TODO: Apply parameters to URI
            return SendRequestAsync<T>(new Uri(link.HRef), HttpMethod.Get);
        }

        public Task<T> PostAsync<T>(Uri uri, object data)
        {
            return SendRequestAsync<T>(uri, HttpMethod.Post, data: data, contentType: HalJsonMediaType);
        }

        private async Task<T> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            string accept = HalJsonMediaType,
            object data = null,
            string contentType = null)
        {
            var response = await _connection.SendRequestAsync<T>(
                                    uri,
                                    method,
                                    accept,
                                    data,
                                    contentType);
            return response.BodyAsObject;
        }
    }
}
