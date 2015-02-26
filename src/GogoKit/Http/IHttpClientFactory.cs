using System.Collections.Generic;
using System.Net.Http;

namespace GogoKit.Http
{
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> instance configured with the handlers provided and with an
        /// <see cref="HttpClientHandler"/> as the innermost handler.
        /// </summary>
        /// <param name="handlers">An ordered list of <see cref="DelegatingHandler"/> instances to be invoked as an 
        /// <see cref="HttpRequestMessage"/> travels from the <see cref="HttpClient"/> to the network and an 
        /// <see cref="HttpResponseMessage"/> travels from the network back to <see cref="HttpClient"/>.
        /// The handlers are invoked in a top-down fashion. That is, the first entry is invoked first for 
        /// an outbound request message but last for an inbound response message.</param>
        /// <returns>An <see cref="HttpClient"/> instance with the configured handlers.</returns>
        HttpClient CreateClient(IEnumerable<DelegatingHandler> handlers);
    }
}
