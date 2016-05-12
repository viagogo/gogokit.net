using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidPurchaseActionException : ApiErrorException
    {
        public InvalidPurchaseActionException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}