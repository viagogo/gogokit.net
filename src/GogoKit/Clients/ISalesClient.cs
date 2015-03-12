using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ISalesClient
    {
        Task<PagedResource<Sale>> GetAsync(int page, int pageSize);
    }
}