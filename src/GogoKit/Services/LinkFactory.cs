using System;
using System.Threading.Tasks;
using HalKit.Models.Response;

namespace GogoKit.Services
{
    public class LinkFactory : ILinkFactory
    {
        private readonly Uri _viagogoApiRootEndpoint;

        public LinkFactory(Uri viagogoApiRootEndpoint)
        {
            Requires.ArgumentNotNull(viagogoApiRootEndpoint, nameof(viagogoApiRootEndpoint));

            _viagogoApiRootEndpoint = viagogoApiRootEndpoint;
        }

        public async Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args)
        {
            Requires.ArgumentNotNull(relativeUriFormat, nameof(relativeUriFormat));

            var relativeUri = string.Format(relativeUriFormat, args);
            var href = new Uri(_viagogoApiRootEndpoint, $"{_viagogoApiRootEndpoint}/{relativeUri}");

            return new Link { HRef = href.ToString() };
        }
    }
}