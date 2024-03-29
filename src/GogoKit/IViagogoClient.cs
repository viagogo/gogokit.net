﻿using System;
using GogoKit.Clients;
using GogoKit.Services;
using HalKit;

namespace GogoKit
{
    public interface IViagogoClient
    {
        IGogoKitConfiguration Configuration { get; }
        IHalClient Hypermedia { get; }
        IOAuth2TokenStore TokenStore { get; }
        IOAuth2Client OAuth2 { get; }
        IUserClient User { get; }
        IAddressesClient Addresses { get; }
        IPurchasesClient Purchases { get; }
        ISalesClient Sales { get; }
        IShipmentsClient Shipments { get; }
        ICountriesClient Countries { get; }
        ICurrenciesClient Currencies { get; }
        IPaymentMethodsClient PaymentMethods { get; }
        IListingsClient Listings { get; }
        ISellerListingsClient SellerListings { get; }
        IWebhooksClient Webhooks { get; }
        IViagogoCatalogClient Catalog { get; }
    }
}
