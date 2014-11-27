using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class PurchaseStillProcessingException : ApiErrorException
    {
        public PurchaseStillProcessingException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}