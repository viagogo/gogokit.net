using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InternalServerErrorException : ApiErrorException
    {
        public InternalServerErrorException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}
