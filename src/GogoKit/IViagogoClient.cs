using GogoKit.Clients;
using GogoKit.Configuration;
using HalKit;

namespace GogoKit
{
    public interface IViagogoClient
    {
        IConfiguration Configuration { get; }
        IHalClient Hypermedia { get; }
        IOAuth2Client OAuth2 { get; }
        IUserClient User { get; }
        ISearchClient Search { get; }
        IAddressesClient Addresses { get; }
        IPurchasesClient Purchases { get; }
        ICountriesClient Countries { get; }
        ICurrenciesClient Currencies { get; }
        IPaymentMethodsClient PaymentMethods { get; }
        ICategoriesClient Categories { get; }
        IEventsClient Events { get; }
        IListingsClient Listings { get; }
        IVenuesClient Venues { get; }
    }
}
