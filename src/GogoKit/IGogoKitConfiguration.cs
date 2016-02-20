using GogoKit.Enumerations;
using System;
using System.Threading;

namespace GogoKit
{
    public interface IGogoKitConfiguration
    {
        Uri ViagogoApiRootEndpoint { get; set; }

        Uri ViagogoOAuthTokenEndpoint { get; set; }

        /// <summary>
        /// Determines which <see cref="ApiEnvironment"/> should be used. Setting
        /// this will value overrides the values for <see cref="ViagogoApiRootEndpoint"/>
        /// and <see cref="ViagogoOAuthTokenEndpoint"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sandbox-environment</remarks>
        ApiEnvironment ViagogoApiEnvironment { get; set; }

        string LanguageCode { get; set; }

        string CountryCode { get; set; }

        string CurrencyCode { get; set; }

        /// <summary>
        /// Determines whether asynchronous operations should capture the current
        /// <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <remarks>See http://blog.stephencleary.com/2012/02/async-and-await.html#avoiding-context.</remarks>
        bool CaptureSynchronizationContext { get; set; }
    }
}