using System;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public class ResourceLinkComposer : IResourceLinkComposer
    {
        private readonly IApiRootClient _rootClient;
        private readonly IConfiguration _configuration;

        public ResourceLinkComposer(IApiRootClient rootClient, IConfiguration configuration)
        {
            _rootClient = rootClient;
            _configuration = configuration;
        }

        public async Task<Link> ComposeLinkWithAbsolutePathForResource(Uri relativeUri)
        {
            Requires.ArgumentNotNull(relativeUri, "relativeUri");

            var root = await _rootClient.GetAsync().ConfigureAwait(_configuration);
            var baseUrl = new Uri(root.Links["self"].HRef);

            var absoluteResourcePathLink = CreateLink(baseUrl, relativeUri);
            return absoluteResourcePathLink;
        }

        private static Link CreateLink(Uri baseUri, Uri relativeUri)
        {
            return new Link { HRef = new Uri(baseUri, relativeUri).ToString() };
        }
    }
}