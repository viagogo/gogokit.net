using System.Threading.Tasks;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Clients
{
    public interface IUserClient
    {
        Task<User> GetAsync();
    }
}
