using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ApiErrorException : ApiException
    {
        public ApiErrorException(IApiResponse response, ApiError apiError) : base(response)
        {
            Requires.ArgumentNotNull(response, nameof(response));

            Error = apiError;
        }

        public ApiError Error { get; }
    }
}
