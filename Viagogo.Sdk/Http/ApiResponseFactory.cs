using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Viagogo.Sdk.Http
{
    public class ApiResponseFactory : IApiResponseFactory
    {
        private readonly IJsonSerializer _jsonSerializer;

        public ApiResponseFactory(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public async Task<IApiResponse<T>> CreateApiResponseAsync<T>(HttpResponseMessage response)
        {
            string body = null;
            object bodyAsObject = null;
            using (var content = response.Content)
            {
                if (content != null)
                {
                    if (typeof(T) != typeof(byte[]))
                    {
                        body = await response.Content.ReadAsStringAsync();
                        if (body != null && IsJsonContent(response.Content))
                        {
                            bodyAsObject = await _jsonSerializer.DeserializeAsync<T>(body);
                        }
                    }
                    else
                    {
                        bodyAsObject = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                Body = body,
                BodyAsObject = (T)bodyAsObject,
            };

            foreach (var header in response.Headers)
            {
                apiResponse.Headers.Add(header.Key, header.Value.FirstOrDefault());
            }

            if (response.Content != null)
            {
                foreach (var header in response.Content.Headers)
                {
                    apiResponse.Headers.Add(header.Key, header.Value.FirstOrDefault());
                }
            }

            return apiResponse;
        }

        private bool IsJsonContent(HttpContent content)
        {
            if (content.Headers.ContentType == null)
            {
                return false;
            }

            return content.Headers.ContentType.MediaType == "application/hal+json" ||
                   content.Headers.ContentType.MediaType == "application/json" ||
                   content.Headers.ContentType.MediaType == "text/json";
        }
    }
}