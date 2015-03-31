using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;
using HalKit;
using HalKit.Models;
using HalKit.Resources;

namespace GogoKit.Extensions
{
    public static class HalClientExtensions
    {
        public static async Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IDictionary<string, string> parameters) where T : Resource
        {
            Requires.ArgumentNotNull(client, "client");
            Requires.ArgumentNotNull(link, "link");

            var currentLink = link;
            var currentParameters = parameters;
            var items = new List<T>();
            var hasAnotherPage = true;
            while (hasAnotherPage)
            {
                var currentPage = await client.GetAsync<PagedResource<T>>(
                                            currentLink,
                                            currentParameters).ConfigureAwait(client.Configuration);

                items.AddRange(currentPage.Items);

                // Stop passing parameters on subsequent calls since the "next" links
                // will already be assembled with all the parameters needed
                currentParameters = null;
                hasAnotherPage = currentPage.Links.TryGetLink("next", out currentLink);
            }

            return items;
        }
    }
}
