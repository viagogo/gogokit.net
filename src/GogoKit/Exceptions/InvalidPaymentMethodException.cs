using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class InvalidPaymentMethodException : ApiErrorException
    {
        public InvalidPaymentMethodException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}