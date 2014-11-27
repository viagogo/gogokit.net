using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class ListingConflictException : ApiErrorException
    {
        public ListingConflictException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}