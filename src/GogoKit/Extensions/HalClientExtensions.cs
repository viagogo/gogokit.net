using System;
using System.Collections.Generic;
using System.Linq;
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
                request?.Parameters,
                request?.Headers,
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
            var changedResources = await client.GetChangedResourcesInternalAsync<T>(
                                                    link,
                                                    parameters,
                                                    headers,
                                                    cancellationToken).ConfigureAwait(client);
            return changedResources.NewOrUpdatedResources;
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> resources that have changed since
        /// your application's last request.
        /// </summary>
        /// <param name="nextLink">The <see cref="Link"/> that was stored from
        /// your last request.</param>
        public static Task<ChangedResources<T>> GetChangedResourcesAsync<T>(
            this IHalClient client,
            Link nextLink,
            IRequestParameters request,
            CancellationToken cancellationToken) where T : Resource
        {
            return client.GetChangedResourcesAsync<T>(
                nextLink,
                request?.Parameters,
                request?.Headers,
                cancellationToken);
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> resources that have changed since
        /// your application's last request.
        /// </summary>
        /// <param name="nextLink">The <see cref="Link"/> that was stored from
        /// your last request.</param>
        public static Task<ChangedResources<T>> GetChangedResourcesAsync<T>(
            this IHalClient client,
            Link nextLink,
            IDictionary<string, string> parameters,
            IDictionary<string, IEnumerable<string>> headers,
            CancellationToken cancellationToken) where T : Resource
        {
            var parametersWithResourceVersionSort
                = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());
            if (parametersWithResourceVersionSort.ContainsKey("sort"))
            {
                parametersWithResourceVersionSort["sort"] = "resource_version";
            }
            else
            {
                parametersWithResourceVersionSort.Add("sort", "resource_version");
            }

            return client.GetChangedResourcesInternalAsync<T>(
                nextLink,
                parametersWithResourceVersionSort,
                headers,
                cancellationToken);
        }

        private static async Task<ChangedResources<T>> GetChangedResourcesInternalAsync<T>(
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
            var maxPageSize = "10000";
            if (currentParameters.ContainsKey("page_size"))
            {
                currentParameters["page_size"] = maxPageSize;
            }
            else
            {
                currentParameters.Add("page_size", maxPageSize);
            }

            Exception failureException = null;
            var items = new List<T>();
            var deletedItems = new List<T>();
            while (true)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var currentPage = await client.GetAsync<PagedResource<T>>(
                        currentLink,
                        currentParameters,
                        headers,
                        cancellationToken).ConfigureAwait(client.Configuration);

                    items.AddRange(currentPage.Items);
                    if (currentPage.DeletedItems != null)
                    {
                        deletedItems.AddRange(currentPage.DeletedItems);
                    }

                    if (currentPage.NextLink == null ||
                        currentPage.Items.Count == 0 && currentPage.DeletedItems?.Any() != true)
                    {
                        // This is the last page
                        break;
                    }

                    // Stop passing parameters on subsequent calls since the "next" links
                    // will already be assembled with all the parameters needed
                    currentParameters = null;
                    currentLink = currentPage.NextLink;
                }
                catch (Exception ex)
                {
                    failureException = ex;
                    break;
                }
            }

            return new ChangedResources<T>(items, deletedItems, currentLink, failureException);
        }
    }
}
