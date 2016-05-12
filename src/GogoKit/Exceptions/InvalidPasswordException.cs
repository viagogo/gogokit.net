using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidPasswordException : ApiErrorException
    {
        public InvalidPasswordException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}