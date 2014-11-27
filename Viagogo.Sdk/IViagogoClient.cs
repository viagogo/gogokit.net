using Viagogo.Sdk.Clients;

namespace Viagogo.Sdk
{
    public interface IViagogoClient
    {
        IOAuth2Client OAuth2 { get; }
        IApiRootClient Root { get; }
    }
}
