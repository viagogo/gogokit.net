using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class InvalidDeleteException : ApiErrorException
    {
        public InvalidDeleteException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}