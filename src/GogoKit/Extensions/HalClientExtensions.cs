using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models.Request;
using HalKit.Models.Response;

namespace GogoKit.Extensions
{
    public static class HalClientExtensions
    {
        public static Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IRequestParameters request) where T : Resource
        {
            return GetAllPagesAsync<T>(
                client,
                link,
                request.Parameters,
                request.Headers);
        }

        public static async Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IDictionary<string, string> parameters,
            IDictionary<string, IEnumerable<string>> headers) where T : Resource
        {
            Requires.ArgumentNotNull(client, "client");
            Requires.ArgumentNotNull(link, "link");

            var currentLink = link;
            var currentParameters = new Dictionary<string, string>(parameters);
            // Increase page-size to reduce the number of round-trips
            var maxPageSize = "1000";
            if (currentParameters.ContainsKey("page_size"))
            {
                currentParameters["page_size"] = maxPageSize;
            }
            else
            {
                currentParameters.Add("page_size", maxPageSize);
            }

            var items = new List<T>();
            var hasAnotherPage = true;
            while (hasAnotherPage)
            {
                var currentPage = await client.GetAsync<PagedResource<T>>(
                                            currentLink,
                                            currentParameters,
                                            headers).ConfigureAwait(client.Configuration);

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
