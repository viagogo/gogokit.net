using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class InvalidPaymentMethodException : ApiErrorException
    {
        public InvalidPaymentMethodException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}