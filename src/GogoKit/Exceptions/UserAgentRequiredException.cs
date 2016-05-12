using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class UserAgentRequiredException : ApiErrorException
    {
        public UserAgentRequiredException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}
