using GogoKit.Clients;
using GogoKit.Services;
using HalKit;

namespace GogoKit
{
    public interface IViagogoCatalogClient
    {
        IGogoKitConfiguration Configuration { get; }
        IHalClient Hypermedia { get; }
        IOAuth2TokenStore TokenStore { get; }
        IOAuth2Client OAuth2 { get; }
    }
}
