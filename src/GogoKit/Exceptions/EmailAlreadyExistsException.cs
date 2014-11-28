using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class EmailAlreadyExistsException : ApiErrorException
    {
        public EmailAlreadyExistsException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}