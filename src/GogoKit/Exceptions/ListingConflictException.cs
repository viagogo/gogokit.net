using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ListingConflictException : ApiErrorException
    {
        public ListingConflictException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}