using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GogoKit.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(IEnumerable<DelegatingHandler> handlers)
    {
        var clientHandler = new HttpClientHandler();
        if (clientHandler.SupportsAutomaticDecompression)
        {
            clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }

        HttpMessageHandler pipeline = clientHandler;
        foreach (var handler in handlers.Reverse())
        {
            if (handler == null)
            {
                throw new ArgumentException("Handlers contains null handler", "handlers");
            }

            if (handler.InnerHandler != null)
            {
                throw new ArgumentException("Handlers already contain inner handlers", "handlers");
            }

            handler.InnerHandler = pipeline;
            pipeline = handler;
        }

        return new HttpClient(pipeline);
    }
}