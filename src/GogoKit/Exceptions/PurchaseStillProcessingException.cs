using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class PurchaseStillProcessingException : ApiErrorException
    {
        public PurchaseStillProcessingException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}