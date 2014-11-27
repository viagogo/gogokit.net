using System.Threading.Tasks;

namespace Viagogo.Sdk.Json
{
    public interface IJsonSerializer
    {
        Task<string> SerializeAsync(object value);
        Task<T> DeserializeAsync<T>(string json);
    }
}
