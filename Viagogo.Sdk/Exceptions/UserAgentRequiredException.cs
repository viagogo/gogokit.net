using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class UserAgentRequiredException : ApiErrorException
    {
        public UserAgentRequiredException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
