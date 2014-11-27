using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InvalidPurchaseActionException : ApiErrorException
    {
        public InvalidPurchaseActionException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}