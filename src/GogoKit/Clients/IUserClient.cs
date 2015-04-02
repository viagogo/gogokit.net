using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IUserClient
    {
        Task<User> GetAsync();
        Task<User> UpdateAsync(UserUpdate userUpdate);
    }
}
