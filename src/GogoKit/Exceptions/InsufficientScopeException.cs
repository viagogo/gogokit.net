using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InsufficientScopeException : ApiErrorException
    {
        public InsufficientScopeException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}
