using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Http
{
    public interface IApiConnection
    {
        IConnection Connection { get; }

        Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters);

        Task<IReadOnlyList<T>> GetAllPagesAsync<T>(Link link, IDictionary<string, string> parameters)
            where T : Resource;
    }
}