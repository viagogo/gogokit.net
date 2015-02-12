using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class InternalServerErrorException : ApiErrorException
    {
        public InternalServerErrorException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
