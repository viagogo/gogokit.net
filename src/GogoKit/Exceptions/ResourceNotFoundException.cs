using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class ResourceNotFoundException : ApiErrorException
    {
        public ResourceNotFoundException(IApiResponse<ApiError> response) : base(response)
        {
        }

        public override string Message
        {
            get
            {
                return "The requested API resource was not found";
            }
        }
    }
}
