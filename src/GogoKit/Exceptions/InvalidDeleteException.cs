using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidDeleteException : ApiErrorException
    {
        public InvalidDeleteException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}