using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ListingConflictException : ApiErrorException
    {
        public ListingConflictException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}