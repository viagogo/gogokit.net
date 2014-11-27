using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
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
