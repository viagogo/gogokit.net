using Viagogo.Sdk.Clients;
using Viagogo.Sdk.Http;

namespace Viagogo.Sdk
{
    public interface IViagogoClient
    {
        IApiConnection Connection { get; }
        IOAuth2Client OAuth2 { get; }
        IApiRootClient Root { get; }
        IUserClient User { get; }
        ISearchClient Search { get; }
    }
}
