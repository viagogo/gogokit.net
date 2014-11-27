using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viagogo.Sdk.Authentication;

namespace Viagogo.Sdk.Http
{
    public interface IConnection
    {
        Task<IApiResponse<T>> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            string accept,
            object body,
            string contentType);

        ICredentialsProvider CredentialsProvider { get; set; }
    }
}
