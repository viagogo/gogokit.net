using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
{
    public class ValidationFailedException : ApiErrorException
    {
        public ValidationFailedException(IApiResponse<ApiError> response)
            : base(response)
        {
        }
    }
}