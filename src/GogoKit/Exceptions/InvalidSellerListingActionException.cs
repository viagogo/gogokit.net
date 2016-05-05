using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidSellerListingActionException : ApiErrorException
    {
        public InvalidSellerListingActionException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}
