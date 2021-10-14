using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HalKit.Http;

namespace GogoKit.Clients
{
    [Obsolete("This functionality will be removed in v3.0.0")]
    public interface IBatchClient
    {
        Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(IEnumerable<IApiRequest> requests);

        Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(
            IEnumerable<IApiRequest> requests,
            IDictionary<string, string> parameters,
            IDictionary<string, IEnumerable<string>> headers,
            CancellationToken cancellationToken);
    }
}
