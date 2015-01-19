using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Configuration;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Http
{
    public interface IHypermediaConnection
    {
        IConfiguration Configuration { get; }

        IHttpConnection HttpConnection { get; }

        Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters);

        Task<IReadOnlyList<T>> GetAllPagesAsync<T>(Link link, IDictionary<string, string> parameters)
            where T : Resource;

        Task<T> PostAsync<T>(Link link, IDictionary<string, string> parameters, object body);

        Task<T> PatchAsync<T>(Link link, IDictionary<string, string> parameters, object body);

        Task<T> PutAsync<T>(Link link, IDictionary<string, string> parameters, object body);

        Task<IApiResponse> DeleteAsync(Link link, IDictionary<string, string> parameters);
    }
}