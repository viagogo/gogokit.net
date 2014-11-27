using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viagogo.Sdk.Helpers;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Http
{
    public class ApiConnection : IApiConnection
    {
        private const string HalJsonMediaType = "application/hal+json";

        private readonly IConnection _connection;
        private readonly ILinkResolver _linkResolver;

        public ApiConnection(IConnection connection)
            : this(connection, new LinkResolver())
        {
        }

        public ApiConnection(IConnection connection, ILinkResolver linkResolver)
        {
            Requires.ArgumentNotNull(connection, "connection");

            _connection = connection;
            _linkResolver = linkResolver;
        }

        public IConnection Connection
        {
            get { return _connection; }
        }

        public Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters)
        {
            return SendRequestAsync<T>(HttpMethod.Get, link, parameters);
        }

        private async Task<T> SendRequestAsync<T>(
            HttpMethod method,
            Link link,
            IDictionary<string, string> parameters,
            string accept = HalJsonMediaType,
            object data = null,
            string contentType = null)
        {
            Requires.ArgumentNotNull(method, "method");
            Requires.ArgumentNotNull(link, "link");

            var response = await _connection.SendRequestAsync<T>(
                                    _linkResolver.ResolveLink(link, parameters),
                                    method,
                                    accept,
                                    data,
                                    contentType);
            return response.BodyAsObject;
        }
    }
}
