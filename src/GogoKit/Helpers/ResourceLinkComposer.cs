using System;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public class ResourceLinkComposer : IResourceLinkComposer
    {
        private readonly IApiRootClient _rootClient;

        public ResourceLinkComposer(IApiRootClient rootClient)
        {
            _rootClient = rootClient;
        }

        public async Task<Link> ComposeLinkWithAbsolutePathForResource(Uri relativeUri)
        {
            Requires.ArgumentNotNull(relativeUri, "relativeUri");

            var root = await _rootClient.GetAsync().ConfigureAwait(false);
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