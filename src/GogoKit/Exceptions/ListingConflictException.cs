using GogoKit.Models;
using GogoKit.Models.Response;
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