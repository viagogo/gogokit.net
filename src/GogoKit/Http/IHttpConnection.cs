using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Configuration;

namespace GogoKit.Http
{
    public interface IHttpConnection
    {
        Task<IApiResponse<T>> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            string accept,
            object body,
            string contentType);

        IReadOnlyList<DelegatingHandler> Handlers { get; }

        IConfiguration Configuration { get; }
    }
}
