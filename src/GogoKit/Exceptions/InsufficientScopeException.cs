using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InsufficientScopeException : ApiErrorException
    {
        public InsufficientScopeException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
