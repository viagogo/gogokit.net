using System;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IApiRootClient _rootClient;
        private readonly IConfiguration _configuration;

        public LinkFactory(IApiRootClient rootClient, IConfiguration configuration)
        {
            _rootClient = rootClient;
            _configuration = configuration;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, "relativeUriFormat");

            var root = await _rootClient.GetAsync().ConfigureAwait(_configuration);
            var baseUri = new Uri(root.Links["self"].HRef);
            var relativeUri = new Uri(string.Format(relativeUriFormat, args), UriKind.Relative);

            return new Link {HRef = new Uri(baseUri, relativeUri).ToString()};
        }
    }
}