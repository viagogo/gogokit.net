using GogoKit.Models;
using GogoKit.Models.Response;
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