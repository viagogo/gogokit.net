using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Http;

namespace GogoKit
{
    public interface IViagogoClient
    {
        IConfiguration Configuration { get; }
        IHypermediaConnection Connection { get; }
        IOAuth2Client OAuth2 { get; }
        IApiRootClient Root { get; }
        IUserClient User { get; }
        ISearchClient Search { get; }
        IAddressesClient Addresses { get; }
        IPurchaseClient Purchase { get; }
        ICountryClient Country { get; }
        ICurrencyClient Currency { get; }
        IPaymentMethodsClient PaymentMethods { get; }
        ICategoryClient Category { get; }
        IEventClient Event { get; }
        IListingClient Listing { get; }
        IVenueClient Venue { get; }
    }
}
