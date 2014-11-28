using GogoKit.Http;
using GogoKit.Models;

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