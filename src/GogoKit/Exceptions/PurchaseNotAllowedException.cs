using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class PurchaseNotAllowedException : ApiErrorException
    {
        public PurchaseNotAllowedException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}