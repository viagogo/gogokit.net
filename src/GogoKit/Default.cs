using GogoKit.Enumerations;
using System;
using System.Collections.Generic;

namespace GogoKit
{
    public static class Default
    {
        public static readonly IDictionary<ApiEnvironment, Uri> ViagogoApiRootEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://api.viagogo.net/v2"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.api.viagogo.net/v2")
            };

        public static readonly IDictionary<ApiEnvironment, Uri> ViagogoCatalogApiRootEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://api.viagogo.net/catalog"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.api.viagogo.net/catalog")
            };

        public static readonly IDictionary<ApiEnvironment, Uri> ViagogoOAuthTokenEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://account.viagogo.com/oauth2/token"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.account.viagogo.com/oauth2/token")
            };

        public static readonly IDictionary<ApiEnvironment, Uri> ViagogoAuthorizationEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://account.viagogo.com/authorize"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.account.viagogo.com/authorize")
            };
    }
}
