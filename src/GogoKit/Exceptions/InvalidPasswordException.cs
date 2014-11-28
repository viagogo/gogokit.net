using GogoKit.Http;
using GogoKit.Models;

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