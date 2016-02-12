using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models.Request;
using HalKit.Models.Response;
using GogoKit.Models.Request;

namespace GogoKit
{
    public static class HalClientExtensions
    {
        public static Task<Root> GetRootAsync(this IHalClient client)
        {
            return client.GetRootAsync(new RootRequest());
        }

        public static Task<Root> GetRootAsync(this IHalClient client, RootRequest request)
        {
            return client.GetRootAsync<Root>(request);
        }

        public static Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link) where T : Resource
        {
            return GetAllPagesAsync<T>(client, link, null, null);
        }

        public static Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IRequestParameters request) where T : Resource
        {
            return GetAllPagesAsync<T>(
                client,
                link,
                request,
                CancellationToken.None);
        }

        public static Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IRequestParameters request,
            CancellationToken cancellationToken) where T : Resource
        {
            return GetAllPagesAsync<T>(
                client,
                link,
                request.Parameters,
                request.Headers,
                cancellationToken);
        }

        public static Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IDictionary<string, string> parameters,
            IDictionary<string, IEnumerable<string>> headers) where T : Resource
        {
            return GetAllPagesAsync<T>(
                client,
                link,
                parameters,
                headers,
                CancellationToken.None);
        }

        public static async Task<IReadOnlyList<T>> GetAllPagesAsync<T>(
            this IHalClient client,
            Link link,
            IDictionary<string, string> parameters,
            IDictionary<string, IEnumerable<string>> headers,
            CancellationToken cancellationToken) where T : Resource
        {
            Requires.ArgumentNotNull(client, nameof(client));
            Requires.ArgumentNotNull(link, nameof(link));

            var currentLink = link;
            var currentParameters = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());
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
                cancellationToken.ThrowIfCancellationRequested();

                var currentPage = await client.GetAsync<PagedResource<T>>(
                                            currentLink,
                                            currentParameters,
                                            headers,
                                            cancellationToken).ConfigureAwait(client.Configuration);

                items.AddRange(currentPage.Items);

                // Stop passing parameters on subsequent calls since the "next" links
                // will already be assembled with all the parameters needed
                currentParameters = null;
                currentLink = currentPage.NextLink;
                hasAnotherPage = currentLink != null;
            }

            return items;
        }
    }
}
