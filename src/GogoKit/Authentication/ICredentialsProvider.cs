using System.Threading.Tasks;

namespace GogoKit.Authentication
{
    public interface ICredentialsProvider
    {
        Task<ICredentials> GetCredentialsAsync();
    }
}
