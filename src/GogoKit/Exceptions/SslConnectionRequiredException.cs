using GogoKit.Models;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class SslConnectionRequiredException : ApiErrorException
    {
        public SslConnectionRequiredException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
