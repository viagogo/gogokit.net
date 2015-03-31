using System;
using System.Collections.Generic;
using System.Linq;
using GogoKit.Models;
using HalKit.Models;

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
            var unresolvedParametersWithValues = unresolvedParameters.Where(kv => kv.Value != null);
            var parametersQueryString = string.Join("&", unresolvedParametersWithValues.Select(kv => kv.Key + "=" + kv.Value));
            var resolvedParameters = uriBuilder.Query.Replace("?", "");
            if (NeedsAmpersandBetweenResolvedAndUnresolvedParameters(resolvedParameters, unresolvedParameters))
            {
                resolvedParameters += "&";
            }
            uriBuilder.Query = resolvedParameters + parametersQueryString;

            return uriBuilder.Uri;
        }

        private static bool NeedsAmpersandBetweenResolvedAndUnresolvedParameters(
            string resolvedParameters,
            Dictionary<string, string> unresolvedParameters)
        {
            return unresolvedParameters.Any() && !string.IsNullOrEmpty(resolvedParameters) && !resolvedParameters.EndsWith("&");
        }
    }
}
