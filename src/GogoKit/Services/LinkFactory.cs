using System;
using System.Threading.Tasks;
using HalKit.Models.Response;

namespace GogoKit.Services
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IGogoKitConfiguration _configuration;

        public LinkFactory(IGogoKitConfiguration configuration)
        {
            Requires.ArgumentNotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, nameof(relativeUriFormat));

            var relativeUri = string.Format(relativeUriFormat, args);
            var href = new Uri(_configuration.ViagogoApiRootEndpoint, $"{_configuration.ViagogoApiRootEndpoint}/{relativeUri}");

            return new Link { HRef = href.ToString() };
        }
    }
}