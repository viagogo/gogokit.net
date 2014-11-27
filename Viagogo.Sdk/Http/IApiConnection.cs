using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Models;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Http
{
    public interface IApiConnection
    {
        IConnection Connection { get; }

        Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters);

        Task<IReadOnlyList<T>> GetAllPagesAsync<T>(Link link, IDictionary<string, string> parameters)
            where T : Resource;
    }
}