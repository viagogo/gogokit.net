using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class InsufficientScopeException : ApiErrorException
    {
        public InsufficientScopeException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
