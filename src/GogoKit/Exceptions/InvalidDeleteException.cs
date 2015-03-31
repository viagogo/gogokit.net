using GogoKit.Models;
using HalKit.Http;

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