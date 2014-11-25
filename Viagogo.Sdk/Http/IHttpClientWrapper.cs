using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
