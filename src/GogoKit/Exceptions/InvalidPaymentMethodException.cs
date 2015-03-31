using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidPaymentMethodException : ApiErrorException
    {
        public InvalidPaymentMethodException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}