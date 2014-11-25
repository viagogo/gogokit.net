using System.Threading.Tasks;

namespace Viagogo.Sdk
{
    public interface IJsonSerializer
    {
        Task<string> SerializeAsync(object value);
        Task<T> DeserializeAsync<T>(string json);
    }
}
