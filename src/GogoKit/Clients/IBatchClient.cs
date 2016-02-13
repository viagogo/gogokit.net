using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HalKit.Http;

namespace GogoKit.Clients
{
    public interface IBatchClient
    {
        Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(IEnumerable<IApiRequest> requests);

        Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(
            IEnumerable<IApiRequest> requests,
            CancellationToken cancellationToken);
    }
}
