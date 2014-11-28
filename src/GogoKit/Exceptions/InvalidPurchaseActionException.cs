using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class InvalidPurchaseActionException : ApiErrorException
    {
        public InvalidPurchaseActionException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}