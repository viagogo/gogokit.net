using GogoKit.Clients;
using HalKit;

namespace GogoKit
{
    public interface IViagogoCatalogClient
    {
        IHalClient Hypermedia { get; }
        IEventsClient Events { get; }
        IVenuesClient Venues { get; }
    }
}
