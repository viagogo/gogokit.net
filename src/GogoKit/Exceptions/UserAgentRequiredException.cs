using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class UserAgentRequiredException : ApiErrorException
    {
        public UserAgentRequiredException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
