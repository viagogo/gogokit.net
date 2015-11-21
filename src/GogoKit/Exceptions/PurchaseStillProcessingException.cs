using GogoKit.Models.Response;
using HalKit.Http;

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