using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Authentication;
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

        ICredentialsProvider CredentialsProvider { get; }

        IConfiguration Configuration { get; }
    }
}
