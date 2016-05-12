using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ValidationFailedException : ApiErrorException
    {
        public ValidationFailedException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}