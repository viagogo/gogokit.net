﻿using GogoKit.Clients;
using GogoKit.Http;

namespace GogoKit
{
    public interface IViagogoClient
    {
        IHypermediaConnection Connection { get; }
        IOAuth2Client OAuth2 { get; }
        IApiRootClient Root { get; }
        IUserClient User { get; }
        ISearchClient Search { get; }
        IAddressClient Address { get; }
        IPurchaseClient Purchase { get; }
        ICountryClient Country { get; }
        ICurrencyClient Currency { get; }
        IPaymentMethodClient PaymentMethod { get; }
        ICategoryClient Category { get; }
    }
}
