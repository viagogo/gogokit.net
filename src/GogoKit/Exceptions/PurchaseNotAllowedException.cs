using GogoKit.Http;
using GogoKit.Models;

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