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

            var unresolvedParameters = new Dictionary<string, string>(parameters);
            if (link.Templated)
            {
                foreach (var parameter in parameters)
                {
                    var parameterTemplate = Uri.EscapeDataString("{" + parameter.Key + "}");
                    if (!uriBuilder.Path.Contains(parameterTemplate))
                    {
                        continue;
                    }

                    // Substitute the parameter value into the template
                    uriBuilder.Path = uriBuilder.Path.Replace(parameterTemplate, parameter.Value);
                    unresolvedParameters.Remove(parameter.Key);
                }
            }

            // Any remaining parameters are query string parameters
            var parametersQueryString = string.Join("&", unresolvedParameters.Select(kv => kv.Key + "=" + kv.Value));
            uriBuilder.Query = uriBuilder.Query.Replace("?", "") + parametersQueryString;

            return uriBuilder.Uri;
        }
    }
}
