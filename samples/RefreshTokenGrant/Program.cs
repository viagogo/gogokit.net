using GogoKit;
using GogoKit.Models.Response;
using GogoKit.Services;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RefreshTokenGrant
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string ClientId = "YOUR_CLIENT_ID";
            const string ClientSecret = "YOUR_CLIENT_SECRET";

            // Refresh tokens are issued with every OAuth token that your application
            // receives using the Authorization Code Grant type.
            // see http://developer.viagogo.net/#authorization-code-grant
            const string RefreshToken = "YOUR_REFRESH_TOKEN";

            // You can create your own custom implementation of IOAuth2TokenStore that
            // stores tokens for users in a database or in user cookies, etc
            var oauth2TokenStore = CreateOAuth2TokenStore(RefreshToken).Result;

            var viagogoClient = new ViagogoClient(ClientId,
                                                  ClientSecret,
                                                  new ProductHeaderValue("GogoKit-Samples"),
                                                  new GogoKitConfiguration(),
                                                  oauth2TokenStore);

            Console.WriteLine("Getting current OAuth2 token");
            var currentToken = viagogoClient.TokenStore.GetTokenAsync().Result;

            var tokenExpirationDate = currentToken.IssueDate.AddSeconds(currentToken.ExpiresIn);
            if (tokenExpirationDate >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine("OAuth2 token is expired so refreshing the token");

                // The token is expired so refresh it
                var newAccessToken = viagogoClient.OAuth2.RefreshAccessTokenAsync(currentToken).Result;

                Console.WriteLine("Saving new OAuth2 token");
                viagogoClient.TokenStore.SetTokenAsync(newAccessToken).Wait();

                Console.WriteLine("We've refreshed our token and saved the new one");
            }
            else
            {
                Console.WriteLine("OAuth2 token is still valid so continue using it");
            }

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static async Task<IOAuth2TokenStore> CreateOAuth2TokenStore(string refreshToken)
        {
            // This method just creates an IOAuth2TokenStore that will return an expired
            // token so that the sample can refresh it
            var expiredToken = new OAuth2Token
                               {
                                    ExpiresIn = 1200,
                                    IssueDate = DateTimeOffset.UtcNow.AddSeconds(-1200),
                                    RefreshToken = refreshToken
                               };
            var tokenStore = new InMemoryOAuth2TokenStore();
            await tokenStore.SetTokenAsync(expiredToken);

            return tokenStore;
        }
    }
}
