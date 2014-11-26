using System.Net.Http;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public interface IApiResponseFactory
    {
        Task<IApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response);
    }
}
