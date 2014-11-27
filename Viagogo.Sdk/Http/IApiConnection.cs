using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Http
{
    public interface IApiConnection
    {
        IConnection Connection { get; }

        Task<T> GetAsync<T>(Link link, IDictionary<string, string> parameters);
    }
}