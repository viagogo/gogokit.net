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

        private readonly IConnection _connection;
        private readonly IOAuth2Client _oauth2Client;
        private readonly IApiRootClient _rootClient;
        private readonly IUserClient _userClient;

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product)
            : this(clientId,
                   clientSecret,
                   product,
                   new AutoRefreshingTokenCredentialsProvider(
                       new OAuth2Client(CreateOAuthConnection(clientId, clientSecret, product))))
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            ICredentialsProvider credentialsProvider)
            : this(clientId, clientSecret, product, credentialsProvider, ViagogoApiUrl, ViagogoDotComUrl)
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            ICredentialsProvider credentialsProvider,
            Uri viagogoApiUrl,
            Uri viagogoDotComUrl)
            : this(new Connection(product, credentialsProvider),
                   CreateOAuthConnection(clientId, clientSecret, product),
                   viagogoApiUrl,
                   viagogoDotComUrl)
        {
        }

        public ViagogoClient(IConnection connection, IConnection oauthConnection)
            : this(connection, oauthConnection, ViagogoApiUrl, ViagogoDotComUrl)
        {
        }

        public ViagogoClient(IConnection connection,
                             IConnection oauthConnection,
                             Uri viagogoApiUrl,
                             Uri viagogoDotComUrl)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(oauthConnection, "oauthConnection");
            Requires.ArgumentNotNull(viagogoApiUrl, "viagogoApiUrl");
            Requires.ArgumentNotNull(viagogoDotComUrl, "viagogoDotComUrl");

            _connection = connection;
            _oauth2Client = new OAuth2Client(oauthConnection, viagogoDotComUrl);
            _rootClient = new ApiRootClient(viagogoApiUrl, connection);

            var apiConnection = new ApiConnection(_connection);
            _userClient = new UserClient(_rootClient, apiConnection);
        }

        public IConnection Connection
        {
            get { return _connection; }
        }

        public IOAuth2Client OAuth2
        {
            get { return _oauth2Client; }
        }

        public IApiRootClient Root
        {
            get { return _rootClient; }
        }

        public IUserClient User
        {
            get { return _userClient; }
        }

        private static IConnection CreateOAuthConnection(
            string clientId,
            string clientSecret,
            ProductHeaderValue product)
        {
            return new Connection(
                product,
                new InMemoryCredentialsProvider(new BasicCredentials(clientId, clientSecret)));
        }
    }
}