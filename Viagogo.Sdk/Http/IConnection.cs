using System;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public interface IConnection
    {
        Task<IApiResponse<T>> PostAsync<T>(Uri uri, object body);
    }
}
