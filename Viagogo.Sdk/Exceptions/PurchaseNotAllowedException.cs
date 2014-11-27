using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class PurchaseNotAllowedException : ApiErrorException
    {
        public PurchaseNotAllowedException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}