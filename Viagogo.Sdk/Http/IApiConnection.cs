using System;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public interface IApiConnection
    {
        IConnection Connection { get; }

        Task<T> PostAsync<T>(Uri uri, object data);
    }
}