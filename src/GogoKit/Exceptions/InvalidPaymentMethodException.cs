using GogoKit.Http;
using GogoKit.Models;

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