using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class UserAgentRequiredException : ApiErrorException
    {
        public UserAgentRequiredException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
