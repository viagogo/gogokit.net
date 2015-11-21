using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ApiErrorException : ApiException
    {
        public ApiErrorException(IApiResponse<ApiError> response) : base(response)
        {
            Requires.ArgumentNotNull(response, nameof(response));

            Error = response.BodyAsObject;
        }

        public ApiError Error { get; }
    }
}
