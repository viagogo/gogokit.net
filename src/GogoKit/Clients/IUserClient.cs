using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IUserClient
    {
        Task<User> GetAsync();
    }
}
