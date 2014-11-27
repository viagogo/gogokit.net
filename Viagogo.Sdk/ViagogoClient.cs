using System;
using System.Net.Http.Headers;
using Viagogo.Sdk.Authentication;
using Viagogo.Sdk.Clients;
using Viagogo.Sdk.Http;

namespace Viagogo.Sdk
{
    public class ViagogoClient : IViagogoClient
    {
        public static readonly Uri ViagogoApiUrl = new Uri("https://api.viagogo.net");
        public static readonly Uri ViagogoDotComUrl = new Uri("https://www.viagogo.com");

        private readonly IOAuth2Client _oauth2Client;
        private readonly IApiRootClient _rootClient;

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product)
            : this(product, new Connection(product, new InMemoryCredentialsProvider(new BasicCredentials(clientId, clientSecret))))
        {
        }

        public ViagogoClient(ProductHeaderValue product, IConnection oauthConnection)
            : this(oauthConnection,
                new Connection(product, new AutoRefreshingTokenCredentialsProvider(new OAuth2Client(new ApiConnection(oauthConnection)))))
        {
        }

        public ViagogoClient(IConnection oauthConnection, IConnection connection)
        {
            Requires.ArgumentNotNull(oauthConnection, "oauthConnection");
            Requires.ArgumentNotNull(connection, "connection");

            _oauth2Client = new OAuth2Client(new ApiConnection(oauthConnection));
            _rootClient = new ApiRootClient(ViagogoApiUrl, connection);
        }

        public IOAuth2Client OAuth2
        {
            get { return _oauth2Client; }
        }

        public IApiRootClient Root
        {
            get { return _rootClient; }
        }
    }
}