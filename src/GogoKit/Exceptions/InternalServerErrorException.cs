using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InternalServerErrorException : ApiErrorException
    {
        public InternalServerErrorException(IApiResponse<ApiError> response) : base(response)
        {
        }
    }
}
