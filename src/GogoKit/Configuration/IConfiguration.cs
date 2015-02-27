using System;
using System.Threading;

namespace GogoKit.Configuration
{
    public interface IConfiguration
    {
        Uri ViagogoApiUrl { get; }
        Uri ViagogoOAuthTokenUrl { get; }

        /// <summary>
        /// Determines whether asynchronous operations should capture the current
        /// <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <remarks>See http://blog.stephencleary.com/2012/02/async-and-await.html#avoiding-context.</remarks>
        bool CaptureSynchronizationContext { get; }
    }
}