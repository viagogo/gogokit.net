using System;
using System.Net.Http;
using System.Threading.Tasks;

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
    }
}
