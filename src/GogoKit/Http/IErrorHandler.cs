using System.Net.Http;
using System.Threading.Tasks;

namespace GogoKit.Http
{
    public interface IErrorHandler
    {
        Task ProcessResponseAsync(HttpResponseMessage response);
    }
}
