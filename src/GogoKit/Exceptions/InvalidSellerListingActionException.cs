using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidSellerListingActionException : ApiErrorException
    {
        public InvalidSellerListingActionException(IApiResponse response, ApiError error) : base(response, error)
        {
        }
    }
}
