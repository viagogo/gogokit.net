using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ISalesClient
    {
        Task<PagedResource<Sale>> GetAsync(int page, int pageSize);
    }
}