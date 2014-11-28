using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IApiRootClient
    {
        Task<ApiRoot> GetAsync();
    }
}
