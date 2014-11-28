using GogoKit.Http;
using GogoKit.Models;

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