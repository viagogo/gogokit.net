using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidPasswordException : ApiErrorException
    {
        public InvalidPasswordException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}