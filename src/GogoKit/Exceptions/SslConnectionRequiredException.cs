using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class SslConnectionRequiredException : ApiErrorException
    {
        public SslConnectionRequiredException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
