using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidSaleActionException : ApiErrorException
    {
        public InvalidSaleActionException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}