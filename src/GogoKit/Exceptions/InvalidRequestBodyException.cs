using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidRequestBodyException : ApiErrorException
    {
        public InvalidRequestBodyException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}