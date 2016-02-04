using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalKit.Http;

namespace GogoKit.Clients
{
    public interface IBatchClient
    {
        Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(IEnumerable<IApiRequest> batchRequests);
    }
}
