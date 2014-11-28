using System;
using System.Collections.Generic;
using System.Linq;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public class LinkResolver : ILinkResolver
    {
        public Uri ResolveLink(Link link, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNull(link, "link");

            if (parameters == null || !parameters.Any())
            {
                return new Uri(link.HRef);
            }

            var uriBuilder = new UriBuilder(link.HRef);
            var parametersQueryString = string.Join("&", parameters.Select(kv => kv.Key + "=" + kv.Value));
            uriBuilder.Query = uriBuilder.Query.Replace("?", "") + parametersQueryString;

            return uriBuilder.Uri;
        }
    }
}
