using System;

namespace GogoKit.Configuration
{
    public class Configuration : IConfiguration
    {
        public static readonly IConfiguration Default
            = new Configuration
            {
                ViagogoApiUrl = new Uri("https://api.viagogo.net"),
                ViagogoDotComUrl = new Uri("https://www.viagogo.com"),
                CaptureSynchronizationContext = false
            };

        public Uri ViagogoApiUrl { get; set; }
        public Uri ViagogoDotComUrl { get; set; }
        public bool CaptureSynchronizationContext { get; set; }
    }
}