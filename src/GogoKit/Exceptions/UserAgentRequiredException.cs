using GogoKit.Models;
using GogoKit.Models.Response;
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
