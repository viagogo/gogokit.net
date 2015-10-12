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
            Requires.ArgumentNotNull(halClient, "halClient");

            _halClient = halClient;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, "relativeUriFormat");
            
            var baseUri = _halClient.Configuration.RootEndpoint;
            var relativeUri = new Uri(string.Format(relativeUriFormat, args), UriKind.Relative);

            return new Link {HRef = new Uri(baseUri, relativeUri).ToString()};
        }
    }
}