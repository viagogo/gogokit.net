using GogoKit.Models;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ValidationFailedException : ApiErrorException
    {
        public ValidationFailedException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}