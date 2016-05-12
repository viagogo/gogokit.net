using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class PurchaseNotAllowedException : ApiErrorException
    {
        public PurchaseNotAllowedException(IApiResponse response, ApiError apiError) : base(response, apiError)
        {
        }
    }
}