using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class SslConnectionRequiredException : ApiErrorException
    {
        public SslConnectionRequiredException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}
