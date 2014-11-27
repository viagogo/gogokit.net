using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Viagogo.Sdk.Json
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings
            = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };

        public Task<string> SerializeAsync(object value)
        {
            return Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, Settings));
        }

        public Task<T> DeserializeAsync<T>(string json)
        {
            return Task.Factory.StartNew(
                () => JsonConvert.DeserializeObject<T>(json, new ResourceConverter()));
        }
    }
}
