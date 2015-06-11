using System;
using System.Threading.Tasks;
using HalKit;
using HalKit.Models.Response;
using GogoKit.Models.Response;

namespace GogoKit.Services
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IHalClient _halClient;

        public LinkFactory(IHalClient halClient)
        {
            Requires.ArgumentNotNull(halClient, "halClient");

            _halClient = halClient;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, "relativeUriFormat");

            var root = await _halClient.GetRootAsync<Root>().ConfigureAwait(_halClient);
            var baseUri = new Uri(root.SelfLink.HRef);
            var relativeUri = new Uri(string.Format(relativeUriFormat, args), UriKind.Relative);

            return new Link {HRef = new Uri(baseUri, relativeUri).ToString()};
        }
    }
}