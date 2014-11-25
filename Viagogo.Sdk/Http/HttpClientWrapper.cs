using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient(GetHandlerStack());

            return httpClient.SendAsync(request, cancellationToken);
        }

        private HttpMessageHandler GetHandlerStack()
        {
            var clientHandler = new HttpClientHandler();
            if (clientHandler.SupportsAutomaticDecompression)
            {
                clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return clientHandler;
        }
    }
}