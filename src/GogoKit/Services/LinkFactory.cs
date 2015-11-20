using System;
using System.Threading.Tasks;
using HalKit;
using HalKit.Models.Response;

namespace GogoKit.Services
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IHalClient _halClient;

        public LinkFactory(IHalClient halClient)
        {
            Requires.ArgumentNotNull(halClient, nameof(halClient));

            _halClient = halClient;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, nameof(relativeUriFormat));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var baseUri = new Uri(root.SelfLink.HRef);
            var relativeUri = new Uri(string.Format(relativeUriFormat, args), UriKind.Relative);

            return new Link { HRef = new Uri(baseUri, relativeUri).ToString() };
        }
    }
}