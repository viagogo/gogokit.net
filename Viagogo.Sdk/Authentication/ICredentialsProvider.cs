using System.Threading.Tasks;

namespace Viagogo.Sdk.Authentication
{
    public interface ICredentialsProvider
    {
        Task<ICredentials> GetCredentialsAsync();
    }
}
