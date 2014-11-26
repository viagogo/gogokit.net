using System.Net.Http;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public interface IErrorHandler
    {
        Task ProcessResponseAsync(HttpResponseMessage response);
    }
}
