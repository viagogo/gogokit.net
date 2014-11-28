using GogoKit.Clients;
using GogoKit.Http;

namespace GogoKit
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
