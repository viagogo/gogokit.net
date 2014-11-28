using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class PurchaseStillProcessingException : ApiErrorException
    {
        public PurchaseStillProcessingException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}