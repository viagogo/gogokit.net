using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IUserClient
    {
        Task<User> GetAsync();
        Task<User> GetAsync(UserRequest request);
        Task<User> GetAsync(UserRequest request, CancellationToken cancellationToken);
        Task<User> UpdateAsync(UserUpdate userUpdate);
    }
}
