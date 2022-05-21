using GogoKit.Clients;
using GogoKit.Services;
using HalKit;
using HalKit.Http;

namespace GogoKit
{
    internal class ViagogoCatalogClient : IViagogoCatalogClient
    {
        public ViagogoCatalogClient(IHttpConnection catalogApiConnection)
        {
            Requires.ArgumentNotNull(catalogApiConnection, nameof(catalogApiConnection));

            var linkFactory = new LinkFactory(catalogApiConnection.Configuration.RootEndpoint);
            Hypermedia = new HalClient(catalogApiConnection.Configuration, catalogApiConnection);
            Events = new EventClient(Hypermedia, linkFactory);
            Venues = new VenuesClient(Hypermedia, linkFactory);
        }

        public IHalClient Hypermedia { get; }

        public IEventsClient Events { get; }

        public IVenuesClient Venues { get; }
    }
}